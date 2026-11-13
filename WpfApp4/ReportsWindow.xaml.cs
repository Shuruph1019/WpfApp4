using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
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
    /// Логика взаимодействия для ReportsWindow.xaml
    /// </summary>
    public partial class ReportsWindow : Window
    {
        public ReportsWindow()
        {
            InitializeComponent();
            FromPicker.SelectedDate = DateTime.Today.AddMonths(-1);
            ToPicker.SelectedDate = DateTime.Today;
            ApplyFilter();
        }

        private void ApplyFilter_Click(object sender, RoutedEventArgs e) => ApplyFilter();

        private void ApplyFilter()
        {
            if (FromPicker.SelectedDate == null || ToPicker.SelectedDate == null) return;
            var filtered = DataStorage.Data.UsageRecords
                .Where(r => r.UsageDate >= FromPicker.SelectedDate.Value && r.UsageDate >= ToPicker.SelectedDate.Value)
                .Select(r => new
                {
                    ФИО = DataStorage.Data.Patients.FirstOrDefault(p => p.Id == r.PatientId)?.FullName ?? "?",
                    Отделение = DataStorage.Data.Departments.FirstOrDefault(d => d.Id == DataStorage.Data.Patients.FirstOrDefault(p => p.Id == r.PatientId)?.DepartmentId)?.Name ?? "?",
                    Дата = r.UsageDate.ToString("dd.MM.yyyy"),
                    Препарат = DataStorage.Data.Medications.FirstOrDefault(m => m.Id == r.MedicationId)?.Name ?? "?",
                    Дозировка = r.Dosage,
                    Количество = r.Quantity
                })
                .ToList();

            ReportGrid.ItemsSource = filtered;
            int total = filtered.Sum(x => x.Количество);
            TotalText.Text = $"Итого записей:  {filtered.Count}, Количество:  {total}"; 
        }

        private void Export_Click(object sender, EventArgs e)
        {
            if (ReportGrid.ItemsSource == null) return;
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "CSV|*.csv",
                FileName = $"report_{DateTime.Now:yyyyMMdd}.csv"
            };
            if (sfd.ShowDialog() == true)
            {
                var sb = new StringBuilder();
                sb.AppendLine("ФИО;Отделение;Дата;Препарат;Дозировка;Количество");
                var items = ((IEnumerable<dynamic>)ReportGrid.ItemsSource).ToList();
                foreach (var item in items)
                {
                    sb.AppendLine($"{item.ФИО};{item.Отделение};{item.Дата};{item.Препарат};{item.Дозировка};{item.Количество}");
                }
                File.WriteAllText(sfd.FileName, sb.ToString(), new System.Text.UTF8Encoding(true));
                MessageBox.Show("Экспортированно в CSV.");
            }
        }
    }
}
