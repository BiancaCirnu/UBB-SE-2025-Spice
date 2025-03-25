using SteamProfile.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamProfile.ViewModels
{
    public class ProfileViewModel
    {
        private readonly UserService _userService;
        // .. add all other needed services
        public ProfileViewModel(UserService userService)
        {
            _userService = userService;
        }
    }
}
