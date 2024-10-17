namespace POE_p2_s4.Models
{
    public class Lecturer:User
    {

      
        public  ICollection<Course>? Courses {  get; set; }
        public double? ClaimRating { get; set; }
        public double? HourlyRate { get; set; }
        
  

        public bool submitClaim(Claim claim)
        {
            return true;
        }
        public string[] submitDocument(IFormFile File)
        {
            return ["hey"];
        }

    }
}
