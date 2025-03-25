using SteamProfile.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamProfile.ViewModels
{
    public class CollectionsViewModel
    {
        private readonly CollectionsService _collectionService;

        public CollectionsViewModel(CollectionsService collectionsService)
        {
            _collectionService = collectionsService;
        }
    }
}
