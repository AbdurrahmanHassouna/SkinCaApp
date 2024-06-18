using APIdemo.Models;

namespace APIdemo.DTOs
{
    public class MedicalReportCreateDto
    {
        public int? Id { get; set; }
        public string? UserId { get; set; }
        public string ReportName { get; set; }
        public string? Content { get; set; }
        public ReportType ReportType { get; set; }
        public IFormFile? ReportFile { get; set; }
    }
}
