namespace APIdemo.DTOs
{
    public class DiseaseDto
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public string Specialty { get; set; }
        public string[] Symptoms { get; set; }
        public string[]? Types { get; set; }
        public string[]? Causes { get; set; }
        public string[]? DiagnosticMethods { get; set; }
        public string[]? Prevention { get; set; }
        public byte[]? Image { get; set; }
    }
}
