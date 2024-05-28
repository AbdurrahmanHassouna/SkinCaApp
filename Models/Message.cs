using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System.ComponentModel.DataAnnotations;

namespace APIdemo.Models
{
    public class Message
    {
        public int Id { get; set; }
        [MaxLength(400)]
        public string? Content { get; set; }
        public byte[]? Image { get; set; }
        public int ChatId { get; set; }
        public MStatus Status { get; set; }
        public virtual Chat Chat { get; set; }
        public string SenderId { get; set; }
        public virtual ApplicationUser Sender { get; set; }
    }
    public enum MStatus
    {
        UnSent,
        Sent,
        Read
    }
}