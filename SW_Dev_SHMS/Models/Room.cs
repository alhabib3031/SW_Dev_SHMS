namespace SW_Dev_SHMS.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public bool IsAvailable { get; set; }
        public Hostel Hostel { get; set; }
    }
}
