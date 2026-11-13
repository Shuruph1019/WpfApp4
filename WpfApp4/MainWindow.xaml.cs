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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp4
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MenuStartInput_Click(object sender, RoutedEventArgs e)
        {
            new PatientCardWindow().ShowDialog();
        }

        private void MenuReports_Click(object sender, RoutedEventArgs e)
        {
            new ReportsWindow().ShowDialog();
        }

        private void MenuReferences_Click(object sender, RoutedEventArgs e)
        {
            new ReferencesWindow().ShowDialog();
        }

        private void MenuBackup_Click(object sender, RoutedEventArgs e)
        {
            new BackupWindow().ShowDialog();
        }

        private void MenuExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MenuPatientsList_Click(object sender, RoutedEventArgs e)
        {
            new PatientListWindow().ShowDialog();
        }
    }
}
