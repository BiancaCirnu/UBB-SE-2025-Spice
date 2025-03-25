using SteamProfile.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamProfile.Services
{
    public class WalletService
    {
        private readonly WalletRepository _walletRepository;
        public WalletService(WalletRepository walletRepository)
        {
            _walletRepository = walletRepository ?? throw new ArgumentNullException(nameof(walletRepository));
        }
    }
}
