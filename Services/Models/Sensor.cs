namespace RobotProject.Services.Models;

public class Sensor(int id, string? name)
{
    public int Id { get; set; } = id;
    public string? Name { get; set; } = name;
    public List<Measurement> measurements { get; set; } = [];
}