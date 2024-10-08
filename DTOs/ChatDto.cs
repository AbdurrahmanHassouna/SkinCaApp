﻿using SkinCaApp.Models;
using System.Text.Json.Serialization;

namespace SkinCaApp.DTOs
{
    public class ChatDto
    {
        public int? Id { get; set; }
        public List<ChatUserDto>? Users { get; set; }
        public List<MessageDto>? Messages { get; set; }
    }
}
