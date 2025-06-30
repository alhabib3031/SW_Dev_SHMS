using System.ComponentModel.DataAnnotations;

namespace SW_Dev_SHMS.Models
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }
        public List<string> PaymentMethods { get; set; } = new List<string>
        { 
            "Bank Transfer", "PayPal"
        };

        public Request? Request { get; set; }
    }
}
