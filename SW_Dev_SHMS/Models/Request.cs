namespace SW_Dev_SHMS.Models
{
    public class Request
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsResolved { get; set; }
        public DormStudent DormStudent { get; set; }
    }
}
