using APIdemo.Models;
using System.Text.Json.Serialization;

namespace APIdemo.DTOs
{
    public class MedicalReportDto
    {
        public MedicalReportDto(MedicalReport medicalReport)
        {
            Id = medicalReport.Id;
            Report=medicalReport.Report;
            Content = medicalReport.Content;
            ReportName = medicalReport.ReportName;
            ReportType = medicalReport.ReportType;
            Report=medicalReport.Report;
        }
        public int? Id { get; set; }
        public string? UserId { get; set; }
        public string ReportName { get; set; }
        public string? Content { get; set; }
        public byte[]? Report { get; set; }
        public ReportType ReportType { get; set; }
        [JsonIgnore]
        public IFormFile? ReportFile { get; set; }
    }
}
