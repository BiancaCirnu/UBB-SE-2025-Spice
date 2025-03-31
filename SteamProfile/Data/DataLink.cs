using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamProfile.Data
{
    public sealed partial class DataLink : IDisposable
    {
        private static readonly Lazy<DataLink> instance = new(() => new DataLink());
        private readonly string connectionString;
        private SqlConnection? sqlConnection;
        private bool disposed;

        private DataLink()
        {
            try
            {
                var config = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();

                string? localDataSource = config["LocalDataSource"];
                string? initialCatalog = config["InitialCatalog"];
                string? userId = config["UserId"];
                string? password = config["Password"];

                if (string.IsNullOrWhiteSpace(localDataSource) || string.IsNullOrWhiteSpace(initialCatalog))
                {
                    throw new ConfigurationErrorsException("Database connection settings are missing in appsettings.json");
                }

                connectionString = $"Data Source={localDataSource};Initial Catalog={initialCatalog};User ID={userId};Password={password};TrustServerCertificate=True;";
                // Test the connection immediately
                using var testConnection = new SqlConnection(connectionString);
                testConnection.Open();
            }
            catch (SqlException ex)
            {
                throw new DatabaseConnectionException("Failed to establish database connection. Please check your connection settings.", ex);
            }
            catch (Exception ex)
            {
                throw new ConfigurationErrorsException("Failed to initialize database connection.", ex);
            }
        }

        public static DataLink Instance => instance.Value;

        private SqlConnection GetConnection()
        {
            if (disposed)
            {
                throw new ObjectDisposedException(nameof(DataLink));
            }

            if (sqlConnection == null || sqlConnection.State == ConnectionState.Closed)
            {
                sqlConnection = new SqlConnection(connectionString);
            }

            return sqlConnection;
        }

        public T? ExecuteScalar<T>(string storedProcedure, SqlParameter[]? sqlParameters = null)
        {
            try
            {
                using var connection = GetConnection();
                connection.Open();

                using var command = new SqlCommand(storedProcedure, connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                if (sqlParameters != null)
                {
                    command.Parameters.AddRange(sqlParameters);
                }

                var result = command.ExecuteScalar();
                return result == DBNull.Value ? default : (T)Convert.ChangeType(result, typeof(T));
            }
            catch (SqlException ex)
            {
                throw new DatabaseOperationException($"Database error during ExecuteScalar operation: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new DatabaseOperationException($"Error during ExecuteScalar operation: {ex.Message}", ex);
            }
        }

        public DataTable ExecuteReader(string storedProcedure, SqlParameter[]? sqlParameters = null)
        {
            try
            {
                using var connection = GetConnection();
                connection.Open();

                using var command = new SqlCommand(storedProcedure, connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                if (sqlParameters != null)
                {
                    command.Parameters.AddRange(sqlParameters);
                }

                using var reader = command.ExecuteReader();
                var dataTable = new DataTable();
                dataTable.Load(reader);
                return dataTable;
            }
            catch (SqlException ex)
            {
                throw new DatabaseOperationException($"Database error during ExecuteReader operation: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new DatabaseOperationException($"Error during ExecuteReader operation: {ex.Message}", ex);
            }
        }

        public int ExecuteNonQuery(string storedProcedure, SqlParameter[]? sqlParameters = null)
        {
            try
            {
                using var connection = GetConnection();
                connection.Open();

                using var command = new SqlCommand(storedProcedure, connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                if (sqlParameters != null)
                {
                    command.Parameters.AddRange(sqlParameters);
                }

                return command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw new DatabaseOperationException($"Database error during ExecuteNonQuery operation: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new DatabaseOperationException($"Error during ExecuteNonQuery operation: {ex.Message}", ex);
            }
        }

        public async Task<int> ExecuteNonQueryAsync(string storedProcedure, SqlParameter[] sqlParameters = null)
        {
            try
            {
                using var connection = GetConnection();
                await connection.OpenAsync();

                using var command = new SqlCommand(storedProcedure, connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                if (sqlParameters != null)
                {
                    command.Parameters.AddRange(sqlParameters);
                }

                return await command.ExecuteNonQueryAsync();
            }
            catch (SqlException ex)
            {
                throw new DatabaseOperationException($"Database error during ExecuteNonQueryAsync operation: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new DatabaseOperationException($"Error during ExecuteNonQueryAsync operation: {ex.Message}", ex);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (sqlConnection != null)
                    {
                        if (sqlConnection.State == ConnectionState.Open)
                        {
                            sqlConnection.Close();
                        }
                        sqlConnection.Dispose();
                        sqlConnection = null;
                    }
                }
                disposed = true;
            }
        }
    }

    public class DatabaseConnectionException : Exception
    {
        public DatabaseConnectionException(string message) : base(message) { }
        public DatabaseConnectionException(string message, Exception innerException) 
            : base(message, innerException) { }
    }

    public class DatabaseOperationException : Exception
    {
        public DatabaseOperationException(string message) : base(message) { }
        public DatabaseOperationException(string message, Exception innerException) 
            : base(message, innerException) { }
    }
}
