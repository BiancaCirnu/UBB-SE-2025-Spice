using SteamProfile.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamProfile.ViewModels
{
    public class FeaturesViewModel
    {
        private readonly FeaturesService _featuresService;

        public FeaturesViewModel(FeaturesService featuresService)
        {
            _featuresService = featuresService;
        }
    }
}
