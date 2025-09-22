using System.Collections.Generic;
using System.Threading.Tasks;

namespace AirlineManagement.Service
{
    public interface IService<T>
    {
        Task<List<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
        Task GetByCodeAsync(string depAirport);
    }
}
