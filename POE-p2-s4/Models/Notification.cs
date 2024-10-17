using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

namespace POE_p2_s4.Models
{

    public class Notification
    {
        public string Id { get; set; }
        [Required,StringLength(50)]
        public string NotificationType { get; set; }
        public string? Description { get; set; }
        [Required]
        public DateOnly Date { get; set; }
        [Required]
        public TimeOnly Time { get; set; }
        [Required]
        public bool Opened { get; set; }
        [Required]
        public bool isSender { get; set; }
        [Required]
        public string Department { get; set; }
        public string UserId { get; set; }
        public User UserNav { get; set; }

        public void SendNotification(string message)
        {

        }

        public List<string> ReceiveNotification(string message) 
        { 
            return new List<string> { "hey"};
        }

       
    }
}
