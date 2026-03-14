using Microsoft.EntityFrameworkCore;
using SimpleWebApp.Data;
using SimpleWebApp.Models;

namespace SimpleWebApp.Services
{
    public class RequestRepository : IRequestRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public RequestRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Request>> GetAllAsync()
        {
            return await _dbContext.Requests
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<Request?> GetByIdAsync(int id)
        {
            return await _dbContext.Requests.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task AddAsync(Request request)
        {
            await _dbContext.Requests.AddAsync(request);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Request request)
        {
            _dbContext.Requests.Update(request);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Request request)
        {
            _dbContext.Requests.Remove(request);
            await _dbContext.SaveChangesAsync();
        }
    }
}
