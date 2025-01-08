namespace RobotProject.Services.Models;

public class Measurement(int id, int value, DateTime timestamp)
{
    public int Id { get; set; } = id;
    public int Value { get; set; } = value;
    public DateTime Timestamp { get; set; } = timestamp;
}