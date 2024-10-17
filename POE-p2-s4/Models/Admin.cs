using System.ComponentModel.DataAnnotations;

namespace POE_p2_s4.Models
{
    public class Admin:User
    {
        [Required]
        public bool? hasFullAccess { get; set; }

        public void reviewClaim(string claimId)
        {

        }
        public void approveClaim(string claimId) { }

        public void rejectClaim(string claimId) { }

    }
}
