using System.ComponentModel.DataAnnotations;
using SW_Dev_SHMS.Models.Entities;

namespace SW_Dev_SHMS.Data.Entities
{
    /// <summary>
    /// Enumeration for available payment methods in the system.
    /// Simple payment options for school project.
    /// </summary>
    public enum PaymentMethod
    {
        BankTransfer,
        PayPal
    }

    /// <summary>
    /// Payment entity representing simple payment records for accommodation applications.
    /// Minimal implementation suitable for school project requirements.
    /// All relationships are configured via navigation properties for lazy loading.
    /// </summary>
    public class Payment
    {
        /// <summary>
        /// Primary key for the Payment entity.
        /// Auto-generated integer identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Payment method selected by the student.
        /// Simple enum with two basic options.
        /// </summary>
        [Required(ErrorMessage = "Payment method is required")]
        [Display(Name = "Payment Method")]
        public PaymentMethod PaymentMethod { get; set; }

        /// <summary>
        /// Indicates if the payment has been completed.
        /// Simple boolean for school project (no complex processing).
        /// </summary>
        [Display(Name = "Payment Completed")]
        public bool IsCompleted { get; set; } = false;

        /// <summary>
        /// Date when the payment was made.
        /// </summary>
        [Display(Name = "Payment Date")]
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Navigation property to the Student who made this payment.
        /// EF Core will handle the foreign key internally.
        /// </summary>
        public virtual Student? Student { get; set; }

        /// <summary>
        /// Navigation property to the Request this payment is for.
        /// EF Core will handle the foreign key internally.
        /// </summary>
        public virtual Request? Request { get; set; }
    }
}