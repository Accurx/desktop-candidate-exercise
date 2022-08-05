using System;

namespace DesktopApp
{
    public class PatientVaccineInfo
    {
        public PatientVaccineInfo(int patientId, string nhsNumber, string firstName, string lastName, string status, VaccineType? vaccineType, DateTimeOffset vaccineDate)
        {
            PatientId = patientId;
            NhsNumber = nhsNumber;
            FirstName = firstName;
            LastName = lastName;
            Status = status;
            VaccineType = vaccineType;
            VaccineDate = vaccineDate;
        }

        public int PatientId { get; }
        public string NhsNumber { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Status { get; }
        public VaccineType? VaccineType { get; }
        public DateTimeOffset VaccineDate { get; }
    }
}