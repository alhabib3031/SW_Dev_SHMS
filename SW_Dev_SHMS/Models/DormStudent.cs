using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW_Dev_SHMS.Models
{
    public class DormStudent
    {
        [Key]
        public int StudentId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }

        [ForeignKey("Room")]
        public int RoomId { get; set; }
        public Room? Room { get; set; }
        public DormManager? DormManager { get; set; }
        public ICollection<Notification>? Notification { get; set; }

        [ForeignKey("Request")]
        public int RequestId { get; set; }
        public Request? Request { get; set; }
    }
}
