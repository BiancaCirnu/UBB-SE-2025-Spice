using SteamProfile.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamProfile.ViewModels
{
    public class LoginViewModel
    {
        private readonly UserService _userService;
        public LoginViewModel(UserService userService)
        {
            _userService = userService;
        }
    }
}
