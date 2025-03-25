using SteamProfile.Repositories;
using SteamProfile.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamProfile.ViewModels
{
    public class WalletViewModel
    {
        private readonly WalletService _walletRepository;

        public WalletViewModel(WalletService walletRepository)
        {
            _walletRepository = walletRepository;
        }
    }
}
