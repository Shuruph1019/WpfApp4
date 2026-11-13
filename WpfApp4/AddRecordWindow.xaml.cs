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
    /// Логика взаимодействия для AddRecordWindow.xaml
    /// </summary>
    public partial class AddRecordWindow : Window
    {
        public RecordRow Result { get; private set; }
        public AddRecordWindow()
        {
            InitializeComponent();
            MedCombo.ItemsSource = DataStorage.Data.Medications;
            DatePicker.SelectedDate = DateTime.Today;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (MedCombo.SelectedItem == null || DatePicker.SelectedDate == null || string.IsNullOrWhiteSpace(DosageBox.Text))
            {
                MessageBox.Show("Заполните все поля.");
                return;
            }
            int qty = 1;
            int.TryParse(QtyBox.Text, out qty);
            var med = (Medication)MedCombo.SelectedItem;
            Result = new RecordRow
            {
                MedicationId = med.Id,
                MedicationName = med.Name,
                UsageDate = DatePicker.SelectedDate.Value,
                Dosage = DosageBox.Text,
                Quantity = qty
            };
            DialogResult = true;
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }
    }
}
