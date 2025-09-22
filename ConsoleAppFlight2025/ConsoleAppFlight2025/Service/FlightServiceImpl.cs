using AirlineManagement.Model;
using AirlineManagement.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AirlineManagement.Service
{
    public class FlightServiceImpl : IService<Flight>
    {
        private readonly IRepository<Flight> repository;

        public FlightServiceImpl(IRepository<Flight> repository)
        {
            this.repository = repository;
        }



        public Task AddAsync(Flight entity) => repository.AddAsync(entity);
        public Task DeleteAsync(int id) => repository.DeleteAsync(id);
        public Task<List<Flight>> GetAllAsync() => repository.GetAllAsync();

        public Task GetByCodeAsync(string depAirport)
        {
            throw new NotImplementedException();
        }

        public Task<Flight> GetByIdAsync(int id) => repository.GetByIdAsync(id);
        public Task UpdateAsync(Flight entity) => repository.UpdateAsync(entity);
    }
}
