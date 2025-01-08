using RobotProject.Services.Enums;

namespace RobotProject.Services.Models;

public class Notification(int id, string description, int level, DateTime? timestamp)
{
    public int Id { get; set; } = id;
    public string Description { get; set; } = description;
    public NotificationType Level { get; set; } = (NotificationType)level;
    public DateTime? Timestamp { get; set; } = timestamp;
}