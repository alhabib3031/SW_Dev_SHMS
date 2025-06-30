using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace SW_Dev_SHMS.Models
{
    public class Student : IdentityUser
    {
        // [Key]
        // public int StudentId { get; set; }
        public string? FullName { get; set; }
        // public string? Email { get; set; }
        // public string? PhoneNumber { get; set; }

        [ForeignKey("Room")]
        public int RoomId { get; set; }
        public Room? Room { get; set; }
        public Manager? DormManager { get; set; }
        public ICollection<Notification>? Notification { get; set; }

        [ForeignKey("Request")]
        public int RequestId { get; set; }
        public Request? Request { get; set; }
    }
}
