using SteamProfile.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamProfile.Services
{
    public class FeaturesService
    {
        private readonly FeaturesRepository _featuresRepository;

        public FeaturesService(FeaturesRepository featuresRepository)
        {
            _featuresRepository = featuresRepository ?? throw new ArgumentNullException(nameof(featuresRepository));
        }
    }
}
