using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW_Dev_SHMS.Models
{
    public class Request
    {
        [Key]
        public int RequestId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsResolved { get; set; }
        public Student? DormStudent { get; set; }

        [ForeignKey("Payment")]
        public int PaymentId { get; set; }
        public Payment? Payment { get; set; }
    }
}
