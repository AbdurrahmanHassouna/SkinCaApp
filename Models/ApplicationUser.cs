using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APIdemo.Models
{
    public class ApplicationUser:IdentityUser
    {
        [MaxLength(50)]
        public string FirstName { get; set; }
        [MaxLength(100)]
        public string LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public Governorate Governorate { get; set; }
        public double? Latitude {  get; set; }
        public double? Longitude { get; set; }
        public string? Address { get; set; }
        public byte[]? ProfilePicture { get; set; }
        public virtual List<MedicalReport> MedicalReports { get; set; }
        public virtual List<BookMark> BookMarks { get; set; }
        public virtual List<Disease> Diseases { get; set; }
        public virtual List<ModelResult> ModelResults { get; set; }
        public virtual List<Banner> Banners { get; set; }
        public virtual ICollection<ApplicationUserChat> ApplicationUserChats { get; set; }
        [JsonIgnore]
        public virtual List<Chat> Chats { get; set; }
        [JsonIgnore]
        public virtual DoctorInfo DoctorInfo { get; set; }
    }
    public enum Governorate
    {
        Cairo,
        Alexandria,
        Giza,
        Qalyubia,
        PortSaid,
        Suez,
        Damietta,
        Dakahlia,
        Sharkia,
        Gharbia,
        KafrElSheikh,
        Monufia,
        Beheira,
        Ismailia,
        Menoufia,
        BeniSuef,
        Fayoum,
        Minya,
        Asyut,
        Sohag,
        Qena,
        Luxor,
        Aswan,
        RedSea,
        NewValley,
        Matrouh,
        NorthSinai,
        SouthSinai,
        Helwan 
    }
}
