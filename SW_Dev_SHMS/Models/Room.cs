using System.ComponentModel.DataAnnotations.Schema;

namespace SW_Dev_SHMS.Models
{
    public class Room
    {
        public int RoomId { get; set; }
        public string? Type { get; set; }
        public bool IsAvailable { get; set; }

        [ForeignKey("Hostel")]
        public int HostelId { get; set; }
        public Hostel? Hostel { get; set; }
        public ICollection<DormStudent>? DormStudents { get; set; }
    }
}
