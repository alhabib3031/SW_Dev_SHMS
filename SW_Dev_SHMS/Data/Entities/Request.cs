using System.ComponentModel.DataAnnotations;

namespace SW_Dev_SHMS.Models.Entities;
    /// <summary>
    /// Enumeration for accommodation application status workflow.
    /// Represents the different stages of an application from creation to completion.
    /// </summary>
    public enum ApplicationStatus
    {
        Draft,
        PendingPayment,
        Submitted,
        UnderReview,
        Approved,
        Rejected,
        Assigned
    }

    /// <summary>
    /// Request entity representing student applications for hostel accommodation.
    /// Students submit applications to get assigned to their preferred hostel.
    /// Application must be paid for before it can be processed by managers.
    /// All relationships are configured via navigation properties for lazy loading.
    /// </summary>
    public class Request
    {
        /// <summary>
        /// Primary key for the Request entity.
        /// Auto-generated integer identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Academic year for which the accommodation is requested.
        /// Indicates the period for which the student needs hostel accommodation.
        /// Example: "2024-2025", "2023-2024".
        /// </summary>
        [Required(ErrorMessage = "Academic year is required")]
        [StringLength(20, ErrorMessage = "Academic year cannot exceed 20 characters")]
        [Display(Name = "Academic Year")]
        public string AcademicYear { get; set; } = string.Empty;

        /// <summary>
        /// Additional remarks or special requirements from the student.
        /// Optional field for any specific needs or circumstances.
        /// </summary>
        [StringLength(500, ErrorMessage = "Remarks cannot exceed 500 characters")]
        [Display(Name = "Remarks")]
        public string? Remarks { get; set; }

        /// <summary>
        /// Current status of the accommodation application using enum.
        /// Tracks the progress from creation to room assignment.
        /// Provides type safety and cleaner code compared to string values.
        /// </summary>
        [Display(Name = "Application Status")]
        public ApplicationStatus Status { get; set; } = ApplicationStatus.Draft;

        /// <summary>
        /// Indicates whether the application fee has been paid.
        /// Must be true before the application can be submitted for processing.
        /// Simple boolean approach for school project (no real payment processing).
        /// </summary>
        [Display(Name = "Payment Completed")]
        public bool IsPaymentCompleted { get; set; } = false;

        /// <summary>
        /// Date and time when the accommodation application was created.
        /// Automatically set when the application is first created.
        /// </summary>
        [Display(Name = "Application Date")]
        public DateTime ApplicationDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Navigation property to the Student who submitted this accommodation application.
        /// Represents the one-to-one relationship: Request -> Student.
        /// Each application is submitted by exactly one student.
        /// EF Core will handle the foreign key internally.
        /// </summary>
        public virtual Student? Student { get; set; }

        /// <summary>
        /// Navigation property to the Hostel that the student wants to be assigned to.
        /// Represents the many-to-one relationship: Request -> Hostel.
        /// Multiple applications can be for the same hostel.
        /// EF Core will handle the foreign key internally.
        /// </summary>
        public virtual Hostel? PreferredHostel { get; set; }
    }