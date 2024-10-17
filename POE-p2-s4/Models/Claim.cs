
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace POE_p2_s4.Models
{
    public class Claim
    {
        public string Id { get; set; }
        public string ClaimType { get; set; }
        public string Description { get; set; }
        public DateTime ClaimDate { get; set; }
        public double HoursWorked { get; set; }
        public double? ClaimExpenses { get; set; }
        [NotMapped]
        public IFormFile? Document { get; set; }
        public string ClaimStatus { get; set; }

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
