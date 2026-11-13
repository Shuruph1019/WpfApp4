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
    public class RecordRow
    {
        public int MedicationId { get; set; }
        public string MedicationName { get; set; }
        public DateTime UsageDate { get; set; }
        public string Dosage { get; set; }
        public int Quantity { get; set; }
    }
    /// <summary>
    /// Логика взаимодействия для PatientCardWindow.xaml
    /// </summary>
    public partial class PatientCardWindow : Window
    {
        private int? _editPatientId = null;
        private List<RecordRow> rows = new List<RecordRow>();
        public PatientCardWindow()
        {
            InitializeComponent();
            DepartmentCombo.ItemsSource = DataStorage.Data.Departments;
            DepartmentCombo.DisplayMemberPath = "Name";
            RecordsGrid.ItemsSource = rows;
        }

        public PatientCardWindow(int patientId)
        {
            InitializeComponent();
            _editPatientId = patientId;
            DepartmentCombo.ItemsSource = DataStorage.Data.Departments;
            DepartmentCombo.DisplayMemberPath = "Name";
            RecordsGrid.ItemsSource = rows;
            var patient = DataStorage.Data.Patients.FirstOrDefault(p => p.Id == patientId);
            if (patient != null)
            {
                FioBox.Text = patient.FullName;
                BirthDatePicker.SelectedDate = patient.BirthDate;
                DepartmentCombo.SelectedItem = DataStorage.Data.Departments.FirstOrDefault(d => d.Id == patient.DepartmentId);
                var patientRecords = DataStorage.Data.UsageRecords.Where(r => r.PatientId == patientId);
                foreach (var r in patientRecords)
                {
                    var med = DataStorage.Data.Medications.FirstOrDefault(m => m.Id == r.MedicationId);
                    rows.Add(new RecordRow
                    {
                        MedicationId = r.MedicationId,
                        MedicationName = med?.Name ?? "?",
                        UsageDate = r.UsageDate,
                        Dosage = r.Dosage,
                        Quantity = r.Quantity,
                    });
                }
                RecordsGrid.ItemsSource = null;
                RecordsGrid.ItemsSource = rows;
            }
        }

        private void AddRecord_Click(object sender, RoutedEventArgs e)
        {
            if (DataStorage.Data.Medications.Count == 0)
            {
                MessageBox.Show("Сначала добавьте препараты в спрвочнике.");
                return;
            }

            var addWindow = new AddRecordWindow();
            if (addWindow.ShowDialog() == true)
            {
                rows.Add(addWindow.Result);
                RecordsGrid.ItemsSource = null;
                RecordsGrid.ItemsSource = rows;
            }
        }

        private void SavePatient_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FioBox.Text) || BirthDatePicker.SelectedDate == null || DepartmentCombo.SelectedItem == null)
            {
                MessageBox.Show("Заполните ФИО, дату рожд и отделение.");
                return;
            }

            var dept = (Department)DepartmentCombo.SelectedItem;
            int newPatientId = DataStorage.Data.Patients.Any()
                ? DataStorage.Data.Patients.Max(p => p.Id) + 1 : 1;
            var patient = new Patient()
            {
                Id = newPatientId,
                FullName = FioBox.Text,
                BirthDate = BirthDatePicker.SelectedDate.Value,
                DepartmentId = dept.Id
            };
            DataStorage.Data.Patients.Add(patient);
            foreach (var r in rows)
            {
                int recId = DataStorage.Data.UsageRecords.Any()
                    ? DataStorage.Data.UsageRecords.Max(x => x.Id) + 1 : 1;
                DataStorage.Data.UsageRecords.Add(new UsageRecord
                {
                    Id = recId,
                    PatientId = newPatientId,
                    MedicationId = r.MedicationId,
                    UsageDate = r.UsageDate,
                    Dosage = r.Dosage,
                    Quantity = r.Quantity,
                });
            }

            DataStorage.Save();
            MessageBox.Show("Карточка сохранена!");
            this.Close();
        }

        private void Back_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    private void DeleteRecord_Click(object sender, EventArgs e)
        {
            if (RecordsGrid.SelectedItem is RecordRow selectedRecord)
            {
                var result = MessageBox.Show($"Удалить препарат {selectedRecord.MedicationName} из карточки?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    rows.Remove(selectedRecord);
                    RecordsGrid.ItemsSource = null;
                    RecordsGrid.ItemsSource = rows;
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выделите строку с препаратом в таблице, которую хотите удалить.", "Внимание");
            }
        }
    }
}