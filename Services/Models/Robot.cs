namespace RobotProject.Services.Models;
public class Robot(int id, string name)
{
    //Database variables
    public int Id { get; } = id;
    public string Name { get; set; } = name;
    public List<Sensor> Sensors { get; set; } = [];
    public List<Notification> Notifications { get; set; } = [];
    public List<Reminder> Reminders { get; set; } = [];
    public List<Medicine> Medicines { get; set; } = [];

    //Mqtt Variables
    public string Status { get; set; } = "Disconnected";
    public double BatteryPercentage { get; set; } = 0;
}