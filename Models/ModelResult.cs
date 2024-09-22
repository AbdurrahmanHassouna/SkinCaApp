using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace SkinCaApp.Models
{
    public class ModelResult
    {
        public int Id { get; set; }
        public byte[] Data { get; set; }
        [Range(0,100)]
        public short Confidence { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}