using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW_Dev_SHMS.Models
{
    public class Notification
    {
        [Key]
        public int NotificationId { get; set; }
        public string? Title { get; set; }
        public string? Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }

        [ForeignKey("Manager")]
        public int ManagerId { get; set; }
        public DormManager? Manager { get; set; }

        [ForeignKey("Student")]
        public int StudentId { get; set; }
        public DormStudent? Student { get; set; }


    }
}
