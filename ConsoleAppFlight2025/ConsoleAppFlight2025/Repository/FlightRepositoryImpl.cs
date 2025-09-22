using AirlineManagement.Model;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;

namespace AirlineManagement.Repository
{
    public class FlightRepositoryImpl : IRepository<Flight>
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["CsWinSql"].ConnectionString;

        public async Task<List<Flight>> GetAllAsync()
        {
            var flights = new List<Flight>();
            using SqlConnection conn = new SqlConnection(connectionString);
            await conn.OpenAsync();

            string query = "SELECT * FROM Flight ORDER BY FlightId";
            using SqlCommand cmd = new SqlCommand(query, conn);
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                flights.Add(new Flight
                {
                    FlightId = (int)reader["FlightId"],
                    DepAirport = reader["DepAirport"].ToString(),
                    DepDate = (DateTime)reader["DepDate"],
                    DepTime = (TimeSpan)reader["DepTime"],
                    ArrAirport = reader["ArrAirport"].ToString(),
                    ArrDate = (DateTime)reader["ArrDate"],
                    ArrTime = (TimeSpan)reader["ArrTime"]
                });
            }
            return flights;
        }


        public async Task AddAsync(Flight flight)
        {
            using SqlConnection conn = new SqlConnection(connectionString);
            await conn.OpenAsync();

            using SqlCommand cmd = new SqlCommand("sp_AddFlight", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@DepAirport", flight.DepAirport);
            cmd.Parameters.AddWithValue("@DepDate", flight.DepDate);
            cmd.Parameters.AddWithValue("@DepTime", flight.DepTime);
            cmd.Parameters.AddWithValue("@ArrAirport", flight.ArrAirport);
            cmd.Parameters.AddWithValue("@ArrDate", flight.ArrDate);
            cmd.Parameters.AddWithValue("@ArrTime", flight.ArrTime);

            await cmd.ExecuteNonQueryAsync();
        }


        public async Task<Flight> GetByIdAsync(int id)
        {
            using SqlConnection conn = new SqlConnection(connectionString);
            await conn.OpenAsync();

            string query = "SELECT * FROM Flight WHERE FlightId=@id";
            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id);

            using SqlDataReader reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Flight
                {
                    FlightId = (int)reader["FlightId"],
                    DepAirport = reader["DepAirport"].ToString(),
                    DepDate = (DateTime)reader["DepDate"],
                    DepTime = (TimeSpan)reader["DepTime"],
                    ArrAirport = reader["ArrAirport"].ToString(),
                    ArrDate = (DateTime)reader["ArrDate"],
                    ArrTime = (TimeSpan)reader["ArrTime"]
                };
            }
            return null;
        }

        

        public async Task UpdateAsync(Flight flight)
        {
            using SqlConnection conn = new SqlConnection(connectionString);
            await conn.OpenAsync();

            string query = @"UPDATE Flight SET 
                             DepAirport=@dep, DepDate=@depDate, DepTime=@depTime,
                             ArrAirport=@arr, ArrDate=@arrDate, ArrTime=@arrTime
                             WHERE FlightId=@id";
            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", flight.FlightId);
            cmd.Parameters.AddWithValue("@dep", flight.DepAirport);
            cmd.Parameters.AddWithValue("@depDate", flight.DepDate);
            cmd.Parameters.AddWithValue("@depTime", flight.DepTime);
            cmd.Parameters.AddWithValue("@arr", flight.ArrAirport);
            cmd.Parameters.AddWithValue("@arrDate", flight.ArrDate);
            cmd.Parameters.AddWithValue("@arrTime", flight.ArrTime);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using SqlConnection conn = new SqlConnection(connectionString);
            await conn.OpenAsync();

            string query = "DELETE FROM Flight WHERE FlightId=@id";
            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<object> GetByCodeAsync(string airportCode)
        {
            using SqlConnection conn = new SqlConnection(connectionString);
            await conn.OpenAsync();

            string query = "SELECT * FROM Airport WHERE AirportCode=@code";
            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@code", airportCode);

            using SqlDataReader reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new
                {
                    AirportCode = reader["AirportCode"].ToString(),
                    City = reader["City"].ToString(),
                    Country = reader["Country"].ToString()
                };
            }
            return null;
        }



        public List<Flight> GetAll()
        {
            throw new NotImplementedException();
        }

        public Flight GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Add(Flight entity)
        {
            throw new NotImplementedException();
        }

        public void Update(Flight entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public object GetByCode(string airportCode)
        {
            throw new NotImplementedException();
        }
    }
}
