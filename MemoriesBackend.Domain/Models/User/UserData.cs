﻿namespace MemoriesBackend.Domain.Models.User
{
    public class UserData
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
