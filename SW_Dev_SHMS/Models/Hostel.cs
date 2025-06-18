namespace SW_Dev_SHMS.Models
{
    public class Hostel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public List<Room> Rooms { get; set; }
    }
}
