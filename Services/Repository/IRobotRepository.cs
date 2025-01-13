using RobotProject.Services.Models;

namespace RobotProject.Services.Repository;
public interface IRobotRepository
{
    void InsertRobot(Robot robot, string tables = "all");
    void UpdateRobot(Robot robot, string tables = "all");
    List<Robot> GetAllRobots();
    bool IsRobotInDatabase(int id);
    void InsertMeasurement(string msg);
    void InsertNotification(string message);
    void InsertReminder(Reminder reminder, int robotId);
    void InsertMedicine(Medicine medicine, int robotId);
    void RemoveReminder(Reminder reminder, int robotId);
    void RemoveMedicine(Medicine medicine, int robotId);
    List<Reminder> GetReminders();
}