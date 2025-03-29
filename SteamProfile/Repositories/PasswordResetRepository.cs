using SteamProfile.Data;
using System;
using System.Data.SqlClient;

namespace SteamProfile.Repositories
{
    public interface IPasswordResetRepository
    {
        void StoreResetCode(int userId, string code, DateTime expiryTime);
        bool VerifyResetCode(string email, string code);
        bool ResetPassword(string email, string code, string hashedPassword);
        void CleanupExpiredCodes();
    }

    public class PasswordResetRepository : IPasswordResetRepository
    {
        private readonly DataLink _dataLink;

        public PasswordResetRepository(DataLink dataLink)
        {
            _dataLink = dataLink ?? throw new ArgumentNullException(nameof(dataLink));
        }

        public void StoreResetCode(int userId, string code, DateTime expiryTime)
        {
            try
            {
                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@userId", userId),
                    new SqlParameter("@resetCode", code),
                    new SqlParameter("@expirationTime", expiryTime)
                };

                _dataLink.ExecuteNonQuery("StorePasswordResetCode", parameters);
            }
            catch (DatabaseOperationException ex)
            {
                throw new RepositoryException($"Failed to store reset code for user {userId}.", ex);
            }
        }

        public bool VerifyResetCode(string email, string code)
        {
            try
            {
                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@email", email),
                    new SqlParameter("@resetCode", code)
                };

                var result = _dataLink.ExecuteScalar<int>("VerifyResetCode", parameters);
                return result == 1;
            }
            catch (DatabaseOperationException ex)
            {
                throw new RepositoryException("Failed to verify reset code.", ex);
            }
        }

        public bool ResetPassword(string email, string code, string hashedPassword)
        {
            try
            {
                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@email", email),
                    new SqlParameter("@resetCode", code),
                    new SqlParameter("@newPassword", hashedPassword)
                };

                var result = _dataLink.ExecuteScalar<int>("ResetPassword", parameters);
                return result == 1;
            }
            catch (DatabaseOperationException ex)
            {
                throw new RepositoryException("Failed to reset password.", ex);
            }
        }

        public void CleanupExpiredCodes()
        {
            try
            {
                _dataLink.ExecuteNonQuery("CleanupResetCodes");
            }
            catch (DatabaseOperationException ex)
            {
                throw new RepositoryException("Failed to cleanup expired reset codes.", ex);
            }
        }
    }
} 