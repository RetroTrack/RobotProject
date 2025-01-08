namespace RobotProject.Services.Models;

public class Reminder(int id, string description, DateTime timestamp)
{
    public int Id { get; set; } = id;
    public string Description { get; set; } = description;
    public DateTime Timestamp { get; set; } = timestamp;
}