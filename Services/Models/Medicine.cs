namespace RobotProject.Services.Models;

public class Medicine(int id, string name, DateTime timestamp)
{
    public int Id { get; set; } = id;
    public string Name { get; set; } = name;
    public DateTime Timestamp { get; set; } = timestamp;
}