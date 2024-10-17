namespace POE_p2_s4.Models
{
    public class Course
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public DateTime LastUpdated { get; set; }
        public User UserNav { get; set; }
        public string UserId { get; set; }
       

        public bool AddCourse()
        {
            return false;
        }

        public bool UpdateCourse()
        {


            return false;
        }

        public string DeleteCourse()
        {
            return "";
        }

    }
}
