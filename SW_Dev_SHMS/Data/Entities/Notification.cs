using System.ComponentModel.DataAnnotations;

namespace SW_Dev_SHMS.Models.Entities;

public class Notification
{
        /// <summary>
        /// Primary key for the Notification entity.
        /// Auto-generated integer identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Title or subject of the notification.
        /// Brief description of what the notification is about.
        /// </summary>
        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
        [Display(Name = "Notification Title")]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Main content or body of the notification message.
        /// Contains the detailed information being communicated to students.
        /// </summary>
        [Required(ErrorMessage = "Message is required")]
        [StringLength(500, ErrorMessage = "Message cannot exceed 500 characters")]
        [Display(Name = "Message")]
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Date and time when the notification was created.
        /// Automatically set when the notification is first created.
        /// </summary>
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Indicates whether the notification has been read by the recipient.
        /// False by default, updated to True when student views the notification.
        /// </summary>
        [Display(Name = "Is Read")]
        public bool IsRead { get; set; } = false;

        /// <summary>
        /// Navigation property to the Manager who created this notification.
        /// Represents the many-to-one relationship: Notification -> Manager.
        /// Multiple notifications can be created by the same manager.
        /// EF Core will handle the foreign key internally.
        /// </summary>
        public virtual Manager? Manager { get; set; }

        /// <summary>
        /// Navigation property to the Student who receives this notification.
        /// Represents the many-to-one relationship: Notification -> Student.
        /// Multiple notifications can be sent to the same student.
        /// EF Core will handle the foreign key internally.
        /// </summary>
        public virtual Student? Student { get; set; }
}