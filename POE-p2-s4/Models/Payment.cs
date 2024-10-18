namespace POE_p2_s4.Models
{
    public class Payment // Upcoming unused feature to be applied in part 3
    {
        public string Id { get; set; }
        public DateTime DateTime { get; set; }
        public string paymentMethod { get; set; }

        public string hrId { get; set; }
        public HR authorizedBy { get; set; }
    
       public void ProcessPayment()
        {

        }
    }
}
