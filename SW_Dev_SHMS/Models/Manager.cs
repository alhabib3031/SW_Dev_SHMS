using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace SW_Dev_SHMS.Models
{
    public class Manager : IdentityUser
    {
        // [Key]
        // public int ManagerId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }

        [ForeignKey("Hostel")]
        public int HostelId { get; set; }
        public Hostel? Hostel { get; set; }
        public ICollection<Student>? DormStudents { get; set; }
        public ICollection<Notification>? Notification { get; set; }
    }
}
