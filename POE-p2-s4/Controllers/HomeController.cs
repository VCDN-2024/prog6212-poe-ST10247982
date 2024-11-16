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

            
            if (user != null && user.UserType == "Lecturer")
            {
             
                List<Claim> claims = _context.Claims
                   .Where(c => c.UserId == user.Id)
                   .Select(c => new Claim
                   {
                       ClaimDate = c.ClaimDate,
                       ClaimType = c.ClaimType,
                       ClaimStatus = c.ClaimStatus
                   })
                   .ToList() ?? new List<Claim>();

                var totalClaims = claims.Count();
                var approvedClaims = claims.Count(c => c.ClaimStatus == "Approved");
                var rejectedClaims = claims.Count(c => c.ClaimStatus == "Rejected");
                var pendingClaims = claims.Count(c => c.ClaimStatus == "Pending");

       
                var courses = _context.Courses
                    .Where(course => course.UserId == user.Id)
                    .ToList() ?? new List<Course>();

   
                ViewData["Claims"] = claims;
                ViewData["TotalClaims"] = totalClaims;
                ViewData["ApprovedClaims"] = approvedClaims;
                ViewData["RejectedClaims"] = rejectedClaims;
                ViewData["PendingClaims"] = pendingClaims;
                ViewData["User"] = user;
                ViewData["Courses"] = courses;
            }
            else if (user != null && user.UserType == "HR")
            {
     
                var pendingClaims = await _context.Claims
                    .Where(c => c.ClaimStatus == "Pending")
                    .Select(c => new
                    {
                        UserEmail = _context.Users.FirstOrDefault(u => u.Id == c.UserId).Email,
                        c.HoursWorked,
                        c.ClaimType
                    })
                    .ToListAsync();

          
                ViewData["PendingClaimsList"] = pendingClaims;
                ViewData["User"] = user; 
            }
            else
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
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
