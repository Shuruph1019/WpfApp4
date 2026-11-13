using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp4
{

    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Medication
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Patient
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public int DepartmentId { get; set; }
    }

    public class UsageRecord
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int MedicationId { get; set; }
        public DateTime UsageDate { get; set; }
        public string Dosage {  get; set; }
        public int Quantity { get; set; }
    }

    public class AppData
    {
        public List<Department> Departments { get; set; }
        public List<Medication> Medications { get; set; }
        public List<Patient> Patients { get; set; }
        public List<UsageRecord> UsageRecords { get; set; }

        public AppData()
        {
            Departments = new List<Department>();
            Medications = new List<Medication>();
            Patients = new List<Patient>();
            UsageRecords = new List<UsageRecord>();
        }
    }
    internal class Models
    {
    }
}
