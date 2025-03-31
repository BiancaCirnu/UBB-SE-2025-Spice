using System.Data;

namespace SteamProfile.Data
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
} 