namespace SW_Dev_SHMS.Models
{
    public class DormStudent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DormManager DormManager { get; set; }
    }
}
