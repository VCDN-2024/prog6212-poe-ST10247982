using System.ComponentModel.DataAnnotations.Schema;

namespace POE_p2_s4.ViewModels
{
    public class Claim
    {
        public string ClaimType { get; set; }
        public string Description { get; set; }
        public DateTime ClaimDate { get; set; }
        public double HoursWorked { get; set; }
        public double? ClaimExpenses { get; set; }

        public byte[]? DocumentBinary { get; set; }
 
        public IFormFile? Document { get; set; }
        public string ClaimStatus { get; set; }
        public string UserId { get; set; }
    }
}
