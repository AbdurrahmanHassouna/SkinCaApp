using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Text.Json.Serialization;

namespace SkinCaApp.Models
{
    public class Chat
    {
        public int Id { get; set; }
        
        public Chat()
        {
            Users=new List<ApplicationUser>();
            Messages=new List<Message>();
        }
        public virtual ICollection<ApplicationUserChat> ApplicationUserChats { get; set; }
        [MaxLength(2),JsonIgnore]
        public  List<ApplicationUser> Users { get; set; }
        public  List<Message> Messages { get; set; }
        
    }
}
