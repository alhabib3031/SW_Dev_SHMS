using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW_Dev_SHMS.Models
{
    public class Hostel
    {
        [Key]
        public int HostelId { get; set; }
        public string? Name { get; set; }
        public string? Location { get; set; }
        public DormManager? DormManager { get; set; }
        public ICollection<Room>? Rooms { get; set; }
    }
}
