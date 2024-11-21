using System;
using System.Collections.Generic;
using FluentValidation.Results;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using POE_p2_s4.Data;
using POE_p2_s4.Models;
using POE_p2_s4.Validation;
using POE_p2_s4.ViewModels;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using System.Data;

namespace POE_p2_s4.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CoursesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Courses
        public async Task<IActionResult> Index()
        {
            // Code Attribution
            //Pro C# 10 with .Net 6
            // Adrew Troelsen, Phil Japsike

            var applicationDbContext = _context.Courses.Include(c => c.UserNav);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .Include(c => c.UserNav)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: Courses/Create
        public IActionResult Create()
        {
            ViewBag.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseVM course)
        {
            Course ValidatedCourse = new Course
            {
                Id = Guid.NewGuid().ToString(),
                Name = course.Name,
                Department = course.Department,
                UserId = course.UserId,
                LastUpdated = DateTime.UtcNow,

            };

            CourseValidation validator = new CourseValidation();
            ValidationResult results = validator.Validate(ValidatedCourse);

            if (!results.IsValid)
            {
                foreach (var failure in results.Errors)
                {
                    ModelState.AddModelError(failure.PropertyName, failure.ErrorMessage);
                }
                return View(course);
            }

            if (!ModelState.IsValid)
            {

                return View(course);
            }

            User user = null;
            if (course.UserId != null)
            {
                user = await _context.Users.FirstOrDefaultAsync<User>(u => u.Id == course.UserId);
            }

            if (user == null)
            {
                ModelState.AddModelError("userStatus", "The current user is not logged in.");
                return Redirect("/Account/Login");
            }

            try
            {
                _context.Courses.Add(ValidatedCourse);
                _context.SaveChanges();


            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Server Error", "Unable to add and save changes to the database, try again later!");
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }


            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", course.UserId);
            ViewData["Id"] = course.Id;
            ViewData["LastUpdated"] = course.LastUpdated;



            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,Department,LastUpdated,UserId")] Course course)
        {
            CourseValidation validator = new CourseValidation();
            ValidationResult results = validator.Validate(course);
            if (!results.IsValid)
            {
                foreach (var failure in results.Errors)
                {
                    ModelState.AddModelError(failure.PropertyName, failure.ErrorMessage);
                }
                return View(course);
            }

            if (id != course.Id)
            {
                return View("NotFound", "Unable to find course!");
            }
            if (course.UserId != null)
            {
                course.UserNav = await _context.Users.FirstOrDefaultAsync(u => u.Id == course.UserId);
            }

            if (!ModelState.IsValid)
            {
                ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", course.UserId);
                return View(course);
            }

            try
            {
                course.LastUpdated = DateTime.Now;
                _context.Update(course);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(course.Id))
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

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .Include(c => c.UserNav)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                ModelState.AddModelError("Delete", "Unable to find the specified course!");
                return View(id);
              
            }
            try
            {
                _context.Courses.Remove(course);
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException ex)
            {
                ModelState.AddModelError("Databaseupdate", "Unable to save changes to the database as the entity is being accessed in multiple instances or is currently being updated!");
            }
           
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(string id)
        {
            return _context.Courses.Any(e => e.Id == id);
        }
    }
}
