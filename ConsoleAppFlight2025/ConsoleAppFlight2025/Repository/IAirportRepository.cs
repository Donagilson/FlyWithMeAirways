using AirlineManagement.Model;
using System.Threading.Tasks;

namespace AirlineManagement.Repository
{
    public interface IAirportRepository : IRepository<Airport>
    {
        Task<Airport> GetByCodeAsync(string code);
    }
}
