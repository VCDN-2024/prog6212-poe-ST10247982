
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POE_p2_s4.Models
{
    public class Claim
    {
        public string Id { get; set; }
        public string ClaimType { get; set; }
        public string Description { get; set; }
        public DateTime ClaimDate { get; set; }
        public decimal HoursWorked { get; set; }
        public Decimal? ClaimExpenses { get; set; }
        public int? LeaveDays { get; set; }
        public int? KilometersTravelled { get; set; }
        public byte[]? DocumentBinary { get; set; }
        [NotMapped]
        public IFormFile? Document { get; set; }
        public string ClaimStatus { get; set; }
        public string UserId { get; set; }
     
        public User? UserNav { get; set; }

        public bool calculateClaimRating()
        {
            return true;
        }

        public double calculatePay()
        {
            return 0;
        }

        public bool ValidateClaim()
        {
            return true;
        }
    }
}
