using System.ComponentModel.DataAnnotations;

namespace POE_p2_s4.Models
{
    public class ProgrammeCo_ordinator:Admin
    {
        [Required]
        public string? Programme {  get; set; }
        public ICollection<Course>? AssignedCourses { get; set; }

        public void reviewCourseClaims()
        {

        }
    }
}
