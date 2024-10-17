using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;



namespace POE_p2_s4.Models
{
    public  class User:IdentityUser
    {
        [Required]
        public string UserType { get; set; }


        
        public List<Claim>? Claims { get; set; }
 
        public List<Notification>? Notifications { get; set; }

        public virtual void SendEmail()
        {

        }
    }
}
