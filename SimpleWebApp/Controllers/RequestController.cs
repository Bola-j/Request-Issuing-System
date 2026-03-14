using Microsoft.AspNetCore.Mvc;
using SimpleWebApp.Models;
using SimpleWebApp.Services;
using System.Text.Json;

namespace SimpleWebApp.Controllers
{
    public class RequestController : Controller
    {
        private const string CustomerRequestIdsSessionKey = "CustomerRequestIds";
        private readonly IRequestService _requestService;

        public RequestController(IRequestService requestService)
        {
            _requestService = requestService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var customerRequestIds = GetCustomerRequestIds();
            var requests = (await _requestService.GetAllAsync())
                .Where(r => customerRequestIds.Contains(r.Id))
                .ToList();

            return View(requests);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new Request());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Request request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            await _requestService.CreateAsync(request);
            AddCustomerRequestId(request.Id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (!CanAccessRequest(id))
            {
                TempData["Error"] = "You can only view or edit your own requests from this device session.";
                return RedirectToAction(nameof(Index));
            }

            var request = await _requestService.GetByIdAsync(id);
            if (request is null)
            {
                return NotFound();
            }

            if (request.Status != RequestStatus.Pending)
            {
                TempData["Error"] = "Only pending requests can be edited.";
                return RedirectToAction(nameof(Index));
            }

            return View(request);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Request request)
        {
            if (id != request.Id)
            {
                return BadRequest();
            }

            if (!CanAccessRequest(id))
            {
                TempData["Error"] = "You can only edit your own requests from this device session.";
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var updated = await _requestService.UpdateAsync(request);
            if (!updated)
            {
                TempData["Error"] = "The request cannot be updated.";
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (!CanAccessRequest(id))
            {
                TempData["Error"] = "You can only delete your own requests from this device session.";
                return RedirectToAction(nameof(Index));
            }

            var deleted = await _requestService.DeleteAsync(id);
            if (!deleted)
            {
                TempData["Error"] = "Only pending requests can be deleted.";
            }
            else
            {
                RemoveCustomerRequestId(id);
            }

            return RedirectToAction(nameof(Index));
        }

        private List<int> GetCustomerRequestIds()
        {
            var json = HttpContext.Session.GetString(CustomerRequestIdsSessionKey);
            if (string.IsNullOrWhiteSpace(json))
            {
                return new List<int>();
            }

            return JsonSerializer.Deserialize<List<int>>(json) ?? new List<int>();
        }

        private void SaveCustomerRequestIds(List<int> requestIds)
        {
            HttpContext.Session.SetString(CustomerRequestIdsSessionKey, JsonSerializer.Serialize(requestIds));
        }

        private void AddCustomerRequestId(int requestId)
        {
            var requestIds = GetCustomerRequestIds();
            if (!requestIds.Contains(requestId))
            {
                requestIds.Add(requestId);
                SaveCustomerRequestIds(requestIds);
            }
        }

        private void RemoveCustomerRequestId(int requestId)
        {
            var requestIds = GetCustomerRequestIds();
            if (requestIds.Remove(requestId))
            {
                SaveCustomerRequestIds(requestIds);
            }
        }

        private bool CanAccessRequest(int requestId)
        {
            return GetCustomerRequestIds().Contains(requestId);
        }
    }
}
