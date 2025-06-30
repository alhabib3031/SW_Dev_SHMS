using System.ComponentModel.DataAnnotations;

namespace SW_Dev_SHMS.Models.Entities;

public class Room
{
    /// <summary>
    /// Primary key for the Room entity.
    /// Auto-generated integer identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Room number or identifier within the hostel.
    /// Required field used to identify the specific room (e.g., "101", "A-205").
    /// </summary>
    [Required(ErrorMessage = "Room number is required")]
    [StringLength(10, ErrorMessage = "Room number cannot exceed 10 characters")]
    [Display(Name = "Room Number")]
    public string RoomNumber { get; set; } = string.Empty;

    /// <summary>
    /// Indicates whether the room is available for new student assignments.
    /// True if room has space (less than 2 students), False if room is full.
    /// Updated automatically based on current student assignments.
    /// </summary>
    [Display(Name = "Is Available")]
    public bool IsAvailable { get; set; } = true;

    /// <summary>
    /// Navigation property to the Hostel that this room belongs to.
    /// Represents the many-to-one relationship: Room -> Hostel.
    /// Multiple rooms belong to one hostel.
    /// EF Core will handle the foreign key internally.
    /// </summary>
    public virtual Hostel? Hostel { get; set; }

    /// <summary>
    /// Collection of students currently assigned to this room.
    /// Represents the one-to-many relationship: Room -> Students.
    /// Maximum 2 students can be assigned to each room.
    /// Navigation property - inverse of Student.Room.
    /// </summary>
    public virtual ICollection<Student>? Students { get; set; }
}