using Microsoft.Identity.Client;

namespace POE_p2_s4.Models
{
    public class Document
    {

        public string Id { get; set; }
        public string DocumentType { get; set; }
        public  byte[] DocumentBinary { get; set; }
        public DateTime DateTime { get; set; }
        public string? ClaimId { get; set; }
        public Claim? ClaimNv { get; set; }
        public string ViewDocument(User user)
        {
         

            
            return "";
        }

    }
}
