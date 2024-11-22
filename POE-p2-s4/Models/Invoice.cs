using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace POE_p2_s4.Models
{
    [Table("dbo.Invoice")]
    public class Invoice
    {
      
      public string Id { get; set; }
      public string Title { get; set; }
      public DateTime CreatedDate { get; set; }
      public bool IsPaymentMade { get; set; }
      [NotMapped]
      public List<Claim> Claims { get; set; }
      public string UserId { get; set; }   
        public User? UserNav { get; set; }
    }
}
