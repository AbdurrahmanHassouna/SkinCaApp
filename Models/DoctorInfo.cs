using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIdemo.Models
{
    public class DoctorInfo
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public int Experience {  get; set; }
        public string Description { get; set; }
        
        [DataType(DataType.Currency)]
        public decimal ClinicFees {  get; set; }
        public string Services {  get; set; }
        public string Specialization { get; set; }
        public virtual ICollection<DoctorWorkingDay> WorkingDays { get; set; }
    }
}
