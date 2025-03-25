using SteamProfile.Services;
using SteamProfile.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamProfile.ViewModels
{
    public class AchievementsViewModel : BaseViewModel
    {
        private readonly AchievementsService _achievementsService;

        public AchievementsViewModel(AchievementsService achievementsService)
        {
            _achievementsService = achievementsService;
        }
    }
}
