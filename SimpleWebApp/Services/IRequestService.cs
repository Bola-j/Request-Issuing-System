using SimpleWebApp.Models;

namespace SimpleWebApp.Services
{
    public interface IRequestService
    {
        Task<List<Request>> GetAllAsync();
        Task<Request?> GetByIdAsync(int id);
        Task CreateAsync(Request request);
        Task<bool> UpdateAsync(Request request);
        Task<bool> DeleteAsync(int id);
        Task<bool> ApproveAsync(int id);
        Task<bool> RejectAsync(int id);
    }
}
