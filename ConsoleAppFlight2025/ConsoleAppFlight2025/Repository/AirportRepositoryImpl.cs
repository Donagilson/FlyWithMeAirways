using AirlineManagement.Model;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;

namespace AirlineManagement.Repository
{
    public class AirportRepositoryImpl : IAirportRepository
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["CsWinSql"].ConnectionString;

        public async Task<List<Airport>> GetAllAsync()
        {
            var airports = new List<Airport>();
            using SqlConnection conn = new SqlConnection(connectionString);
            await conn.OpenAsync();

            string query = "SELECT * FROM Airport";
            using SqlCommand cmd = new SqlCommand(query, conn);
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                airports.Add(new Airport
                {
                    AirportCode = reader["AirportCode"].ToString(),
                    City = reader["City"].ToString(),
                    Country = reader["Country"].ToString()
                });
            }
            return airports;
        }

        public async Task AddAsync(Airport airport)
        {
            using SqlConnection conn = new SqlConnection(connectionString);
            await conn.OpenAsync();

            string query = "INSERT INTO Airport (AirportCode, City, Country) VALUES (@code, @city, @country)";
            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@code", airport.AirportCode);
            cmd.Parameters.AddWithValue("@city", airport.City);
            cmd.Parameters.AddWithValue("@country", airport.Country);

            await cmd.ExecuteNonQueryAsync();
        }


        public async Task<Airport> GetByIdAsync(int id) => null; 

    
        public async Task UpdateAsync(Airport airport) { }
        public async Task DeleteAsync(int id) {}

        public async Task<Airport> GetByCodeAsync(string code)
        {
            using SqlConnection conn = new SqlConnection(connectionString);
            await conn.OpenAsync();

            string query = "SELECT * FROM Airport WHERE AirportCode=@code";
            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@code", code);

            using SqlDataReader reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Airport
                {
                    AirportCode = reader["AirportCode"].ToString(),
                    City = reader["City"].ToString(),
                    Country = reader["Country"].ToString()
                };
            }
            return null;
        }
    }
}
