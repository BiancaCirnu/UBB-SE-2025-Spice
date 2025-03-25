using SteamProfile.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamProfile.Services
{
    public class CollectionsService
    {
        private readonly CollectionsRepository _collectionsRepository;

        public CollectionsService(CollectionsRepository collectionsRepository)
        {
            _collectionsRepository = collectionsRepository ?? throw new ArgumentNullException(nameof(collectionsRepository));
        }

    }
}
