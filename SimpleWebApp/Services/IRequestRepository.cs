using SimpleWebApp.Models;

namespace SimpleWebApp.Services
{
    public interface IRequestRepository
    {
        Task<List<Request>> GetAllAsync();
        Task<Request?> GetByIdAsync(int id);
        Task AddAsync(Request request);
        Task UpdateAsync(Request request);
        Task DeleteAsync(Request request);
    }
}
