using SteamProfile.Models;
using SteamProfile.Repositories;
using System;

namespace SteamProfile.Services
{
    public class SessionService
    {
        private readonly SessionRepository _sessionRepository;
        private readonly UserSession _userSession;

        public SessionService(SessionRepository sessionRepository)
        {
            _sessionRepository = sessionRepository ?? throw new ArgumentNullException(nameof(sessionRepository));
            _userSession = UserSession.Instance;
        }

        public void CreateNewSession(User user)
        {
            var sessionId = _sessionRepository.CreateSession(user.UserId);
            _userSession.UpdateSession(sessionId, user);
        }

        public void EndSession()
        {
            if (_userSession.CurrentSessionId.HasValue)
            {
                _sessionRepository.DeleteSession(_userSession.CurrentSessionId.Value);
                _userSession.ClearSession();
            }
        }

        public User? GetCurrentUser()
        {
            if (!_userSession.CurrentSessionId.HasValue)
            {
                return null;
            }

            return _sessionRepository.GetUserFromSession(_userSession.CurrentSessionId.Value);
        }

        public bool IsUserLoggedIn()
        {
            return _userSession.CurrentSessionId.HasValue;
        }
    }
} 