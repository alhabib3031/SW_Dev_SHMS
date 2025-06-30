namespace SW_Dev_SHMS.Models.Entities;

public class Manager:ApplicationUser
{
    /// <summary>
    /// Navigation property to the Hostel entity that this manager oversees.
    /// Represents the one-to-one relationship: Manager ↔ Hostel.
    /// The relationship is configured from the Hostel side via Hostel.DormManager.
    /// EF Core will handle the foreign key internally.
    /// </summary>
    public virtual Hostel? Hostel { get; set; }

    /// <summary>
    /// Collection of students that are under this manager's supervision.
    /// Represents a one-to-many relationship: Manager -> Students.
    /// Students reference this manager via ManagerId (string) foreign key.
    /// Navigation property - inverse of Student.DormManager.
    /// </summary>
    public virtual ICollection<Student>? AssingedStudents { get; set; }
}