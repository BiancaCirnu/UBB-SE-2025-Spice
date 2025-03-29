using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamProfile.Models
{
    internal class Wallet
    {
        public int WalletId {  get; set; }
        public int UserId {  get; set; }
        public int balance {  get; set; }

        public int points { get; set; }
    
       
    }

}
