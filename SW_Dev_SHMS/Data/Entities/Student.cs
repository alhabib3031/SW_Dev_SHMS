namespace SW_Dev_SHMS.Data.Entities;

public class Student:ApplicationUser
{
    /// <summary>
    /// Navigation property to the Room entity that this student is assigned to.
    /// Represents the many-to-one relationship: Student -> Room.
    /// Multiple students can be assigned to the same room.
    /// EF Core will handle the foreign key internally.
    /// </summary>
    public virtual Room? Room { get; set; }

    /// <summary>
    /// Navigation property to the Manager who supervises this student.
    /// Represents the many-to-one relationship: Student -> Manager.
    /// Students are supervised by the manager of their assigned hostel.
    /// Navigation property - inverse of Manager.DormStudents.
    /// EF Core will handle the foreign key internally.
    /// </summary>
    public virtual Manager? DormManager { get; set; }

    /// <summary>
    /// Navigation property to the Request entity that this student has made.
    /// Represents the one-to-one relationship: Student -> Request.
    /// Each student can have one active request.
    /// EF Core will handle the foreign key internally.
    /// </summary>
    public virtual Request? Request { get; set; }

    /// <summary>
    /// Collection of payments made by this student.
    /// Represents the one-to-many relationship: Student -> Payments.
    /// Students can have multiple payment records.
    /// Navigation property - inverse of Payment.Student.
    /// </summary>
    public virtual ICollection<Payment>? Payments { get; set; }
}