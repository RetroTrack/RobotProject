namespace RobotProject.Services.Models
{
    public class CombinedTableItem
    {
        public int Id { get; set; }
        public string Type { get; set; } = ""; // "Medicine" or "Reminder"
        public string NameOrDescription { get; set; } = "";
        public DateTime Timestamp { get; set; }
    }
}
