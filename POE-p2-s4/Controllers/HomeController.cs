using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POE_p2_s4.Data;
using POE_p2_s4.Models;
using System.Diagnostics;

namespace POE_p2_s4.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<User> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public async Task <IActionResult> Index()
        {
            // Attempt to get the user from UserManager
            var user = await _userManager.GetUserAsync(User);

            // Check if the user is not null and is of type "Lecturer"
            if (user != null && user.UserType == "Lecturer")
            {
                // Safely fetch claims for the user and initialize as an empty list if no claims are found
                List<Claim> claims = _context.Claims
                   .Where(c => c.UserId == user.Id)
                   .Select(c => new Claim
                   {
                       ClaimDate = c.ClaimDate,
                       ClaimType = c.ClaimType,
                       ClaimStatus = c.ClaimStatus
                   })
                   .ToList() ?? new List<Claim>();

                // Safely calculate total claims and status counts
                var totalClaims = claims.Count();
                var approvedClaims = claims.Count(c => c.ClaimStatus == "Approved");
                var rejectedClaims = claims.Count(c => c.ClaimStatus == "Rejected");
                var pendingClaims = claims.Count(c => c.ClaimStatus == "Pending");

                // Safely fetch courses for the user and initialize as an empty list if no courses are found
                var courses = _context.Courses
                    .Where(course => course.UserId == user.Id)
                    .ToList() ?? new List<Course>();

                // Store data in ViewData for use in the view
                ViewData["Claims"] = claims;
                ViewData["TotalClaims"] = totalClaims;
                ViewData["ApprovedClaims"] = approvedClaims;
                ViewData["RejectedClaims"] = rejectedClaims;
                ViewData["PendingClaims"] = pendingClaims;
                ViewData["User"] = user;
                ViewData["Courses"] = courses;
            }
            else
            {
                // Handle the case where the user is null or is not a lecturer (optional redirect or error message)
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

       

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
