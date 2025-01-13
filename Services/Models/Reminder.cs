namespace RobotProject.Services.Models;

public class Reminder(int id, string type, string name, string description, DateTime timestamp)
{
    public int Id { get; set; } = id;
    public string Type { get; set; } = type;
    public string Name { get; set; } = name;
    public string Description { get; set; } = description;
    public DateTime Timestamp { get; set; } = timestamp;
    public int RobotId { get; set; }

    public string GetDescription() {
        if(Description == null) return "";
        return Description;
    }
}
