using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace APIdemo.Models
{
    public class BookMark
    {
        public int Id { get; set; }
        public int DiseaseId {  get; set; }
        public Disease Disease { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

    }
}