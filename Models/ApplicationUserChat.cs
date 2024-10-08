﻿namespace SkinCaApp.Models
{
    public class ApplicationUserChat
    {
        public string UserId {  get; set; }
        public int ChatId {  get; set; }
        public bool IsDeleted { get; set; } = false;
        public ApplicationUser User { get; set; }
        public Chat Chat { get; set; }
    }
}
