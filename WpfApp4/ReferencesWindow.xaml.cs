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
    /// Логика взаимодействия для ReferencesWindow.xaml
    /// </summary>
    public partial class ReferencesWindow : Window
    {
        public ReferencesWindow()
        {
            InitializeComponent();
            RefreshLists();
        }

        private void RefreshLists()
        {
            DepartmentsList.ItemsSource = null;
            DepartmentsList.ItemsSource = DataStorage.Data.Departments.Select(d => d.Name).ToList();
            MedicationsList.ItemsSource = null;
            MedicationsList.ItemsSource = DataStorage.Data.Medications.Select(m  => m.Name).ToList();
        }

        private void AddDepatment_Click(object sender, RoutedEventArgs e)
        {
            string name = Microsoft.VisualBasic.Interaction.InputBox("Название отделения:", "Добавить отделение");
            if (!string.IsNullOrWhiteSpace(name))
            {
                int newId = DataStorage.Data.Departments.Any()
                    ? DataStorage.Data.Departments.Max(d => d.Id) + 1 : 1;
                DataStorage.Data.Departments.Add(new Department { Id = newId, Name = name });
                DataStorage.Save();
                RefreshLists();
            }
        }

        private void DeleteDepartment_Click(object sender, RoutedEventArgs e)
        {
            if (DepartmentsList.SelectedItem == null) return;
            string name = DepartmentsList.SelectedItem.ToString();
            var dept = DataStorage.Data.Departments.FirstOrDefault(d => d.Name == name);
            if (dept == null) return;
            if (DataStorage.Data.Patients.Any(p  => p.Id == dept.Id))
            {
                MessageBox.Show("Нельзя удалить отделение: к нему привязаны пациенты.");
                return;
            }
            DataStorage.Data.Departments.Remove(dept);
            DataStorage.Save();
            RefreshLists();
        }

        private void AddMedication_Click(object sender, RoutedEventArgs e)
        {
            string name = Microsoft.VisualBasic.Interaction.InputBox("");
            if (!string.IsNullOrWhiteSpace(name))
            {
                int newId = DataStorage.Data.Medications.Any()
                    ? DataStorage.Data.Medications.Max(m => m.Id) + 1 : 1;
                DataStorage.Data.Medications.Add(new Medication { Id = newId, Name = name });
                DataStorage.Save();
                RefreshLists();
            }
        }

        private void DeleteMedication_Click(Object sender, RoutedEventArgs e)
        {
            if (MedicationsList.SelectedItem == null) return;
            string name = MedicationsList.SelectedItem.ToString();
            var med = DataStorage.Data.Medications.FirstOrDefault(m => m.Name == name);
            if (med ==  null) return;
            if (DataStorage.Data.UsageRecords.Any(u  => u.MedicationId == med.Id))
            {
                MessageBox.Show("Нельзя удалить препарат, так как он используется в записях.");
                return;
            }
            DataStorage.Data.Medications.Remove(med);
            DataStorage.Save();
            RefreshLists();
        }
    }
}
