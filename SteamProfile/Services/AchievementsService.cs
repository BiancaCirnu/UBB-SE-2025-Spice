using SteamProfile.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamProfile.Services
{
    public class AchievementsService
    {
        private readonly AchievementsRepository _achievementsRepository;

        public AchievementsService(AchievementsRepository achievementsRepository)
        {
            _achievementsRepository = achievementsRepository ?? throw new ArgumentNullException(nameof(achievementsRepository));
        }
    }
}
