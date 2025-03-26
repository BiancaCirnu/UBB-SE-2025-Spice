using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamProfile.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; }
        public string? ProfilePicture { get; set; }
        public string? Description { get; set; }
        public bool IsDeveloper { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLogin { get; set; }

        public void UpdateFrom(User other)
        {
            Email = other.Email;
            Username = other.Username;
            Password = other.Password;
            ProfilePicture = other.ProfilePicture;
            Description = other.Description;
            IsDeveloper = other.IsDeveloper;
            CreatedAt = other.CreatedAt;
            LastLogin = other.LastLogin;
        }
    }
}
