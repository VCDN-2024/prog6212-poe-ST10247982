using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using POE_p2_s4.Data;
using POE_p2_s4.Models;
using POE_p2_s4.Services;
using POE_p2_s4.Validation;
using QuestPDF.Fluent;
using System.Security.Claims;
using Claim = POE_p2_s4.Models.Claim;

namespace POE_p2_s4.Controllers
{
    public class InvoicesController : Controller
    {
        private readonly ApplicationDbContext _context;
        public InvoicesController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult GenerateReport(string ClaimType)
        {
            var claimsQueryable = _context.Claims.AsQueryable();
            List<Claim> claims = claimsQueryable.Where(claim => claim.ClaimType.Equals(ClaimType)).ToList();
            if (claims.Count < 1)
            {

                ModelState.AddModelError("Claims", "Unable to find any claims with type " + ClaimType);
                return RedirectToAction(nameof(Index), "Home");
            }
            Invoice invoice = new Invoice
            {
                Id = Guid.NewGuid().ToString(),
                CreatedDate = DateTime.Now,
                Claims = claims,
                IsPaymentMade = false,
                Title = $"CMCS Report for claims of type: {ClaimType}",
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),

            };
            InvoiceValidation validator = new InvoiceValidation();
            ReportGenerator report = new ReportGenerator(invoice, _context);

            using (var memoryStream = new MemoryStream())
            {
                
               
                report.GeneratePdf(memoryStream);

             
                byte[] pdfBytes = memoryStream.ToArray();

                
                invoice.PDFArray = pdfBytes;

               
            }
              ValidationResult results = validator.Validate(invoice);
            if(!results.IsValid)
            {
                foreach(var failure in  results.Errors)
                {
                    ModelState.AddModelError(failure.PropertyName, failure.ErrorMessage);
                }
                return RedirectToAction(nameof(Index), "Home");
            }
            report.GeneratePdf();
            return View();
        }

    }
}

