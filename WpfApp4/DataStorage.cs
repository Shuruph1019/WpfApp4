using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WpfApp4
{
    internal class DataStorage
    {
        private static string filePath = "medical_data.xml";
        public static AppData Data { get; private set; }

        public static void Load()
        {
            try
            {
                if (File.Exists(filePath))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(AppData));
                    using (FileStream fs = new FileStream(filePath, FileMode.Open))
                    {
                        Data = (AppData)serializer.Deserialize(fs);
                    }
                }
                else
                {
                    Data = new AppData();
                    Data.Departments.Add(new Department { Id = 1, Name = "Хирургия" });
                    Data.Departments.Add(new Department { Id = 2, Name = "Гинекология" });
                    Data.Departments.Add(new Department { Id = 3, Name = "Реанимация" });
                    Data.Medications.Add(new Medication { Id = 1, Name = "B6" });
                    Data.Medications.Add(new Medication { Id = 2, Name = "B12" });
                    Save();
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Ошибка загрузки: " + ex.Message);
                Data = new AppData();
            }
        }

        public static void Save()
        {
            try
            {
                if (Data == null) Data = new AppData();
                XmlSerializer serializer = new XmlSerializer(typeof(AppData));
                using (FileStream fs = new FileStream(filePath, FileMode.Create))
                {
                    serializer.Serialize(fs, Data);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Ошибка сохранения: " + ex.Message);
            }
        }
    }
}
