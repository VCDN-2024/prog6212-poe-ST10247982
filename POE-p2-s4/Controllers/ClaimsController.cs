using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using POE_p2_s4.Data;
using POE_p2_s4.Models;
using POE_p2_s4.Validation;
using POE_p2_s4.ViewModels;
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

            // Code Attribution
            //Pro C# 10 with .Net 6
            // Adrew Troelsen, Phil Japsike
            // Entity Framework
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(); // Ensure the user is logged in
            }

         
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                return NotFound("User not found"); 
            }

            // Start building the claims query
            var claimsQuery = _context.Claims.AsQueryable();

            
            if (user.UserType == "Lecturer")
            {
                claimsQuery = claimsQuery.Where(c => c.UserId == user.Id);
             
            }
            else if (user.UserType == "Admin" || user.UserType == "HR" || user.UserType == "ProgrammeCo_ordinator" || user.UserType == "AcademicManager")
            {
                // Admins, HR, Programme Coordinators, and Academic Managers can see pending claims
                claimsQuery = claimsQuery.Where(c => c.ClaimStatus == "Pending");
            }

            ViewData["UserType"] = user.UserType;

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
            ViewBag.User = _context.Users.FirstOrDefault(u => u.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));
            ViewBag.ClaimTypeOptions = new SelectList(Enum.GetValues(typeof(ClaimType)));
            ViewData["ClaimStatus"] = "Pending";
            return View();
        }

        // POST: Claims/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( ClaimVM claimVM)
        {
            Lecturer user =(Lecturer) await _context.Users.FirstOrDefaultAsync(u => u.Id == claimVM.UserId);
            if (User == null)
            {
                ModelState.AddModelError("User", "User not found in the database!");
                return RedirectToAction(nameof(HomeController.Index));
            }
            ViewBag.User = user;
            Claim claim = new Claim
            {
                Id = Guid.NewGuid().ToString(),
                ClaimType = claimVM.ClaimType,
                LeaveDays = claimVM.LeaveDays,
                KilometersTravelled = claimVM.KilometersTravelled,
                ClaimExpenses = claimVM.ClaimExpenses,
                ClaimDate = claimVM.ClaimDate,
                ClaimStatus = claimVM.ClaimStatus,
                Description = claimVM.Description,
                UserId = claimVM.UserId,
                Document = claimVM.Document,
                DocumentBinary = claimVM.DocumentBinary,
                HoursWorked = claimVM.HoursWorked,
                
            };

            ClaimValidation validator = new ClaimValidation((decimal)user.HourlyRate);
            ValidationResult results = validator.Validate(claim);
            if (!results.IsValid)
            {
                foreach(var failure in results.Errors)
                {
               ModelState.AddModelError(failure.PropertyName,failure.ErrorMessage);
                }
                return View(claimVM);
            }
            if (!ModelState.IsValid)
            {

                return View(claimVM);
            }
            if (claim == null)
            {
                ModelState.AddModelError("Claim", "Missing details for the claim, please re-enter your details.");
            }
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
                     return View(claimVM);
                    }
                }
            try
            {
                _context.Add(claim);
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("Create", "Unable to create claim right now!");
                return View(claimVM);
            }
                return RedirectToAction(nameof(Index));
            
           
          
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
            // Code Attribution
            //Document-ByteArrays
            //https://www.tutlane.com/article/csharp/csharp-convert-file-to-byte-array-with-examples

            string contentType = "application/octet-stream"; 
            var fileContent = claim.DocumentBinary;

            // Assuming you want to provide the file as an attachment download
            return File(fileContent, contentType, $"{claim.Id}.doc");
        }

       public async Task<IActionResult> Approve(string id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }
            Claim claim = await _context.Claims.FirstOrDefaultAsync(claim => claim.Id == id);
            if (claim == null)
            {
                return NotFound();
            }
            claim.ClaimStatus = "Approved";
            try
            {
                _context.Claims.Update(claim);
                _context.SaveChanges();
               
            }
            catch(DbUpdateConcurrencyException ex)
            {
                ModelState.AddModelError("Update", "We are unable to update the claim right now, please try again later");

            }
         return   RedirectToAction(nameof(Index));
        }
        private bool ClaimExists(string id)
        {
            return _context.Claims.Any(e => e.Id == id);
        }
    }
}
