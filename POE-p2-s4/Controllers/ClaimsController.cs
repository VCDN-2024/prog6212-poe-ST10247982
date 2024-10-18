using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using POE_p2_s4.Data;
using POE_p2_s4.Models;
using Claim = POE_p2_s4.Models.Claim;

namespace POE_p2_s4.Controllers
{
    public class ClaimsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClaimsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Claims
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(); // Ensure the user is logged in
            }

            // Fetch the logged-in user based on the userId
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                return NotFound("User not found"); // Return error if user isn't found in the database
            }

            // Start building the claims query
            var claimsQuery = _context.Claims.AsQueryable();

            // Apply query filters based on the user's discriminator value (UserType)
            if (user.UserType == "Lecturer")
            {
                // Lecturers can see only their own claims
                claimsQuery = claimsQuery.Where(c => c.UserId == user.Id);
            }
            else if (user.UserType == "Admin" || user.UserType == "HR" || user.UserType == "ProgrammeCo_ordinator" || user.UserType == "AcademicManager")
            {
                // Admins, HR, Programme Coordinators, and Academic Managers can see pending claims
                claimsQuery = claimsQuery.Where(c => c.ClaimStatus == "Pending");
            }

            // Pass the user type to the view for conditional rendering
            ViewData["UserType"] = user.UserType;

            // Execute the query and return the result to the view
            List<Claim> claims = await claimsQuery.ToListAsync();

            return View(claims);
        }

        // GET: Claims/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var claim = await _context.Claims
                .FirstOrDefaultAsync(m => m.Id == id);
            if (claim == null)
            {
                return NotFound();
            }

            return View(claim);
        }

        // GET: Claims/Create
        public IActionResult Create()
        {
            ViewBag.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["ClaimStatus"] = "Pending";
            return View();
        }

        // POST: Claims/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClaimType,Description,ClaimDate,HoursWorked,ClaimExpenses,ClaimStatus,UserId,Document")] Claim claim)
        {
            if (claim != null)
            {
              
                claim.Id = Guid.NewGuid().ToString();
                if(claim.Document!=null && claim.Document.Length > 0)
                {
                    try
                    {
                        using (var ms = new MemoryStream())
                        {
                            await claim.Document.CopyToAsync(ms);
                            claim.DocumentBinary = ms.ToArray();
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("Document", "Unable to save document, please try again.");
                    }
                }
                _context.Add(claim);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("Claim", "Missing details for the claim, please re-enter your details.");
            }
            return View(claim);
        }

        // GET: Claims/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var claim = await _context.Claims.FindAsync(id);
            claim.UserNav = await _context.Users.FindAsync(claim.UserId);
            ViewData["Username"] = claim.UserNav.UserName;
            ViewData["HoursWorked"] = claim.HoursWorked;
            ViewData["Description"] = claim.Description;
            ViewData["ClaimExpenses"]= claim.ClaimExpenses;
            ViewData["ClaimDate"]=claim.ClaimDate;
            ViewData["ClaimStaus"] = claim.ClaimStatus;
            if (claim == null)
            {
                return NotFound();
            }
            return View(claim);
        }

        // POST: Claims/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,ClaimType,Description,ClaimDate,HoursWorked,ClaimExpenses,DocumentBinary,ClaimStatus")] Claim claim)
        {
            if (id != claim.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(claim);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClaimExists(claim.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(claim);
        }

        public async Task<IActionResult> DownloadDocument(string id)
        {
            var claim = await _context.Claims.FindAsync(id);

            if (claim == null || claim.DocumentBinary == null)
            {
                return NotFound();
            }

            // Set the content type based on the document (e.g., PDF, Word, etc.)
            string contentType = "application/octet-stream"; // Default binary type
            var fileContent = claim.DocumentBinary;

            // Assuming you want to provide the file as an attachment download
            return File(fileContent, contentType, $"{claim.Id}.doc");
        }


        private bool ClaimExists(string id)
        {
            return _context.Claims.Any(e => e.Id == id);
        }
    }
}
