using SteamProfile.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamProfile.Repositories
{
    public class CollectionsRepository
    {
        private readonly DataLink _dataLink;

        public CollectionsRepository(DataLink datalink)
        {
            _dataLink = datalink;

        }
    }
}
