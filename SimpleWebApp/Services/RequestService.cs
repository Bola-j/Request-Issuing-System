using SimpleWebApp.Models;

namespace SimpleWebApp.Services
{
    public class RequestService : IRequestService
    {
        private readonly IRequestRepository _requestRepository;

        public RequestService(IRequestRepository requestRepository)
        {
            _requestRepository = requestRepository;
        }

        public async Task<List<Request>> GetAllAsync()
        {
            return await _requestRepository.GetAllAsync();
        }

        public async Task<Request?> GetByIdAsync(int id)
        {
            return await _requestRepository.GetByIdAsync(id);
        }

        public async Task CreateAsync(Request request)
        {
            request.Status = RequestStatus.Pending;
            request.CreatedAt = DateTime.UtcNow;
            await _requestRepository.AddAsync(request);
        }

        public async Task<bool> UpdateAsync(Request request)
        {
            var existing = await _requestRepository.GetByIdAsync(request.Id);
            if (existing is null || existing.Status != RequestStatus.Pending)
            {
                return false;
            }

            existing.FullName = request.FullName;
            existing.Email = request.Email;
            existing.RequestType = request.RequestType;
            existing.Description = request.Description;

            await _requestRepository.UpdateAsync(existing);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _requestRepository.GetByIdAsync(id);
            if (existing is null || existing.Status != RequestStatus.Pending)
            {
                return false;
            }

            await _requestRepository.DeleteAsync(existing);
            return true;
        }

        public async Task<bool> ApproveAsync(int id)
        {
            var request = await _requestRepository.GetByIdAsync(id);
            if (request is null)
            {
                return false;
            }

            request.Status = RequestStatus.Approved;
            await _requestRepository.UpdateAsync(request);
            return true;
        }

        public async Task<bool> RejectAsync(int id)
        {
            var request = await _requestRepository.GetByIdAsync(id);
            if (request is null)
            {
                return false;
            }

            request.Status = RequestStatus.Rejected;
            await _requestRepository.UpdateAsync(request);
            return true;
        }
    }
}
