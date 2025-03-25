using SteamProfile.Services;
using SteamProfile.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamProfile.ViewModels
{
    public class ConfigurationsViewModel : BaseViewModel
    {
        private readonly UserService _userService;
        
        public ConfigurationsViewModel(UserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }
    }
}
