using POE_p2_s4.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace POE_p2_s4.ViewModels
{
    [NotMapped]
    public class CourseVM
    {
        public string Name { get; set; }
        public string Department { get; set; }
        public DateTime LastUpdated { get; set; }
        public string UserId { get; set; }

    }
}
