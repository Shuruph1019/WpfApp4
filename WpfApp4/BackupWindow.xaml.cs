using System;
using System.IO;
using System.Collections.Generic;
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
    /// Логика взаимодействия для BackupWindow.xaml
    /// </summary>
    public partial class BackupWindow : Window
    {
        public BackupWindow()
        {
            InitializeComponent();
        }

        private void Backup_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "XML|*.xml",
                FileName = $"backup_{DateTime.Now:yyyyMMdd}.xml"
            };
            if (sfd.ShowDialog() == true)
            {
                File.Copy("medical_data.xml", sfd.FileName, true);
                MessageBox.Show("Копия создана.");
            }
        }

        private void Restore_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog { Filter = "XML|*.xml" };
            if (ofd.ShowDialog() == true)
            {
                File.Copy(ofd.FileName, "medical_data.xml", true);
                DataStorage.Load();
                MessageBox.Show("Восстановлено. Перезапустите приложение.");
            }
        }
    }
}
