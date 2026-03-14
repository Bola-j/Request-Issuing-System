using Microsoft.AspNetCore.Mvc;
using SimpleWebApp.Services;

namespace SimpleWebApp.Controllers
{
    public class AdminController : Controller
    {
        private const string AdminSessionKey = "IsAdminAuthenticated";
        private readonly IConfiguration _configuration;
        private readonly IRequestService _requestService;

        public AdminController(IRequestService requestService, IConfiguration configuration)
        {
            _requestService = requestService;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (IsAdminAuthenticated())
            {
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string passcode)
        {
            var configuredPasscode = _configuration["AdminAccess:Passcode"];
            if (string.IsNullOrWhiteSpace(passcode) || passcode != configuredPasscode)
            {
                ModelState.AddModelError(string.Empty, "Invalid admin passcode.");
                return View();
            }

            HttpContext.Session.SetString(AdminSessionKey, "true");
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove(AdminSessionKey);
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (!IsAdminAuthenticated())
            {
                return RedirectToAction(nameof(Login));
            }

            var requests = await _requestService.GetAllAsync();
            ViewBag.PendingCount = requests.Count(r => r.Status == Models.RequestStatus.Pending);
            ViewBag.ApprovedCount = requests.Count(r => r.Status == Models.RequestStatus.Approved);
            ViewBag.RejectedCount = requests.Count(r => r.Status == Models.RequestStatus.Rejected);
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Requests()
        {
            if (!IsAdminAuthenticated())
            {
                return RedirectToAction(nameof(Login));
            }

            var requests = await _requestService.GetAllAsync();
            return View(requests);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id)
        {
            if (!IsAdminAuthenticated())
            {
                return RedirectToAction(nameof(Login));
            }

            await _requestService.ApproveAsync(id);
            return RedirectToAction(nameof(Requests));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(int id)
        {
            if (!IsAdminAuthenticated())
            {
                return RedirectToAction(nameof(Login));
            }

            await _requestService.RejectAsync(id);
            return RedirectToAction(nameof(Requests));
        }

        private bool IsAdminAuthenticated()
        {
            return HttpContext.Session.GetString(AdminSessionKey) == "true";
        }
    }
}
