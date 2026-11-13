using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp4
{
    /// <summary>
    /// Логика взаимодействия для PatientListWindow.xaml
    /// </summary>
    public partial class PatientListWindow : Window
    {
        public PatientListWindow()
        {
            InitializeComponent();
            LoadPatients();
        }

        public class PatientView
        {
            public int Id { get; set; }
            public string FullName { get; set; }
            public DateTime BirthDate { get; set; }
            public string DepartmentName { get; set; }
            public DateTime? LastUsageDate { get; set; }
            public string MedicationsInfo { get; set; }
        }

        private void LoadPatients(string searchText = "")
        {
            var list = new List<PatientView>();
            foreach (var p in DataStorage.Data.Patients)
            {
                if (!string.IsNullOrWhiteSpace(searchText) && !p.FullName.ToLower().Contains(searchText.ToLower())) continue;
                var dept = DataStorage.Data.Departments.FirstOrDefault(d => d.Id == p.DepartmentId);
                var records = DataStorage.Data.UsageRecords.Where(r => r.PatientId == p.Id).ToList();
                DateTime? lastDate = records.Any() ? records.Max(r => r.UsageDate) : (DateTime?)null;
                string medsInfo = string.Join(";", records.Select(r =>
                {
                    var med = DataStorage.Data.Medications.FirstOrDefault(m => m.Id == r.MedicationId);
                    return $"{med?.Name ?? "?"} - {r.Dosage} ({r.UsageDate:dd.MM.yyyy})";
                }));

                list.Add(new PatientView
                {
                    Id = p.Id,
                    FullName = p.FullName,
                    BirthDate = p.BirthDate,
                    DepartmentName = dept?.Name ?? "Не указано",
                    LastUsageDate = lastDate,
                    MedicationsInfo = medsInfo,
                });
            }
            PatientsGrid.ItemsSource = list;
        }

        private void SearchBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            LoadPatients(SearchBox.Text);
        }

        private void Back_Click(object  sender, EventArgs e)
        {
            this.Close();
        }

        private void DeletePatient_Click(object sender, EventArgs e)
        {
            if (PatientsGrid.SelectedItem is PatientView selected)
            {
                var result = MessageBox.Show($"Удалить пациента {selected.FullName} и все его записи?",
                    "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    var recordsToRemove = DataStorage.Data.UsageRecords.Where(r => r.PatientId == selected.Id).ToList();
                    foreach (var r in recordsToRemove)
                        DataStorage.Data.UsageRecords.Remove(r);
                    var patient = DataStorage.Data.Patients.FirstOrDefault(p => p.Id == selected.Id);
                    if (patient != null) DataStorage.Data.Patients.Remove(patient);
                    DataStorage.Save();
                    LoadPatients(SearchBox.Text);
                }
            }
            else
            {
                MessageBox.Show("Выберите пациента из списка.");
            }
        }

        private void AddPatient_Click(object  sender, EventArgs e)
        {
            var cardWindow = new PatientCardWindow();
            if (cardWindow.ShowDialog() == true)
            {
                LoadPatients(SearchBox.Text);
            }
        }

        private void EditPatient_Click(object sender  , EventArgs e)
        {
            if (PatientsGrid.SelectedItem is PatientView selected)
            {
                var cardWindow = new PatientCardWindow(selected.Id);
                if (cardWindow.ShowDialog() == true)
                {
                    LoadPatients(SearchBox.Text);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста,выделите пациента в таблице, чтобы его отредактировать.");
            }
        }
    }
}
