using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamProfile.Models 
{
    public sealed class UserSession
    {
        private static UserSession? _instance;
        private static readonly object _lock = new object();

        private UserSession() { }

        public static UserSession Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new UserSession();
                    }
                    return _instance;
                }
            }
        }

        public Guid? CurrentSessionId { get; private set; }
        public User? CurrentUser { get; private set; }
        public DateTime? SessionExpiresAt { get; private set; }

        public void UpdateSession(Guid sessionId, User user)
        {
            lock (_lock)
            {
                CurrentSessionId = sessionId;
                CurrentUser = user;
                SessionExpiresAt = DateTime.Now.AddHours(24); // 24-hour session
            }
        }

        public void ClearSession()
        {
            lock (_lock)
            {
                CurrentSessionId = null;
                CurrentUser = null;
                SessionExpiresAt = null;
            }
        }

        public bool IsSessionValid()
        {
            lock (_lock)
            {
                return CurrentSessionId.HasValue && 
                       CurrentUser != null && 
                       SessionExpiresAt.HasValue && 
                       DateTime.Now < SessionExpiresAt.Value;
            }
        }
    }
}