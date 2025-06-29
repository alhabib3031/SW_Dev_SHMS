using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW_Dev_SHMS.Models
{
    public class DormManager
    {
        [Key]
        public int ManagerId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }

        [ForeignKey("Hostel")]
        public int HostelId { get; set; }
        public Hostel? Hostel { get; set; }
        public ICollection<DormStudent>? DormStudents { get; set; }
        public ICollection<Notification>? Notification { get; set; }
    }
}
