using System;

namespace SteamProfile.Models
{
    public class UserProfile
    {
        public int ProfileId { get; set; }
        public int UserId { get; set; }
        public string? ProfilePicture { get; set; }
        public string? Bio { get; set; }
        public string? EquippedFrame { get; set; }
        public string? EquippedHat { get; set; }
        public string? EquippedPet { get; set; }
        public string? EquippedEmoji { get; set; }
        public DateTime LastModified { get; set; }
    }
} 