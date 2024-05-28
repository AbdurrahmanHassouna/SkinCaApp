﻿using APIdemo.Models;
using System.Text.Json.Serialization;

namespace APIdemo.DTOs
{
    public class ChatDto
    {
        public int? Id { get; set; }
        public List<UserDto>? Users { get; set; }
        public List<MessageDto>? Messages { get; set; }
    }
}
