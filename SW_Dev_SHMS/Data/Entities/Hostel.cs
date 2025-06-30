
using System.ComponentModel.DataAnnotations;

namespace SW_Dev_SHMS.Models.Entities;

public class Hostel
{
    /// <summary>
    /// Primary key for the Hostel entity.
    /// Auto-generated integer identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Name of the hostel building.
    /// Required field that identifies the hostel (e.g., "Block A", "North Wing").
    /// </summary>
    [Required(ErrorMessage = "Hostel name is required")]
    [StringLength(100, ErrorMessage = "Hostel name cannot exceed 100 characters")]
    [Display(Name = "Hostel Name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Physical address or location of the hostel.
    /// Provides location information for the hostel building.
    /// </summary>
    [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters")]
    [Display(Name = "Address")]
    public string? Address { get; set; }

    /// <summary>
    /// Navigation property to the Manager who oversees this hostel.
    /// Represents the one-to-one relationship: Hostel -> Manager.
    /// Each hostel is managed by exactly one manager.
    /// EF Core will handle the foreign key internally.
    /// </summary>
    public virtual Manager? DormManager { get; set; }

    /// <summary>
    /// Collection of rooms that belong to this hostel.
    /// Represents the one-to-many relationship: Hostel -> Rooms.
    /// A hostel contains multiple rooms for student accommodation.
    /// Navigation property - inverse of Room.Hostel.
    /// </summary>
    public virtual ICollection<Room>? Rooms { get; set; }
}