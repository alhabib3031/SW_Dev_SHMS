namespace SW_Dev_SHMS.Models
{
    public class DormManager
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Hostel Hostel { get; set; }
        public List<DormStudent> DormStudents { get; set; } = new();
        public List<Notification> Notification { get; set; } = new();
    }
}
