namespace APIdemo.Models
{
    public class MedicalReport
    {
        public int Id { get; set; }
        public string UserId {  get; set; }
        public ApplicationUser User { get; set; }
        public string 
        public byte[]? Report { get; set; }
        public ReportType ReportType {  get; set; }

    }
    public enum ReportType
    {
        Allergies = 1,
        FamilyHistory,
        Diagonoses,
        Medications,
        Symptomes,
        LapTests,
        DNATests
    }
}