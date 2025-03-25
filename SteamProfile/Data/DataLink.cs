using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamProfile.Data
{
    public sealed class DataLink
    {
        private static readonly Lazy<DataLink> instance = new(() => new DataLink());

        private readonly string connectionString;
        private readonly SqlConnection sqlConnection;

        private DataLink()
        {
            // Load from appsettings.json
            var config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            string? localDataSource = config["LocalDataSource"];
            string? initialCatalog = config["InitialCatalog"];

            if (string.IsNullOrWhiteSpace(localDataSource) || string.IsNullOrWhiteSpace(initialCatalog))
            {
                throw new Exception("Database connection settings are missing in appsettings.json");
            }

            connectionString = $"Data Source={localDataSource};Initial Catalog={initialCatalog};Integrated Security=True;TrustServerCertificate=True;";
            sqlConnection = new SqlConnection(connectionString);
        }

        public static DataLink Instance => instance.Value;

        private void OpenConnection()
        {
            if (sqlConnection.State != ConnectionState.Open)
                sqlConnection.Open();
        }

        private void CloseConnection()
        {
            if (sqlConnection.State != ConnectionState.Closed)
                sqlConnection.Close();
        }

        public T? ExecuteScalar<T>(string storedProcedure, SqlParameter[]? sqlParameters = null)
        {
            try
            {
                OpenConnection();
                using SqlCommand command = new(storedProcedure, sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                if (sqlParameters != null)
                    command.Parameters.AddRange(sqlParameters);

                var result = command.ExecuteScalar();
                return result == DBNull.Value ? default : (T)Convert.ChangeType(result, typeof(T));
            }
            catch (Exception ex)
            {
                throw new Exception($"Error - ExecuteScalar: {ex.Message}", ex);
            }
            finally
            {
                CloseConnection();
            }
        }

        public DataTable ExecuteReader(string storedProcedure, SqlParameter[]? sqlParameters = null)
        {
            try
            {
                OpenConnection();
                using SqlCommand command = new(storedProcedure, sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                if (sqlParameters != null)
                    command.Parameters.AddRange(sqlParameters);

                using SqlDataReader reader = command.ExecuteReader();
                DataTable dataTable = new();
                dataTable.Load(reader);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error - ExecuteReader: {ex.Message}", ex);
            }
            finally
            {
                CloseConnection();
            }
        }

        public int ExecuteNonQuery(string storedProcedure, SqlParameter[]? sqlParameters = null)
        {
            try
            {
                OpenConnection();
                using SqlCommand command = new(storedProcedure, sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                if (sqlParameters != null)
                    command.Parameters.AddRange(sqlParameters);

                return command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error - ExecuteNonQuery: {ex.Message}", ex);
            }
            finally
            {
                CloseConnection();
            }
        }
    }
}
