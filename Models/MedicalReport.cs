namespace APIdemo.Models
{
    public class MedicalReport
    {
        public int Id { get; set; }
        public string UserId {  get; set; }
        public ApplicationUser User { get; set; }
        public string ReportName {  get; set; }
        public string? Content { get; set; }
        public byte[]? Report { get; set; }
        public ReportType ReportType {  get; set; }

    }
    public enum ReportType
    {
        FamilyHistory,
        Medication,
        LapTest
    }
}