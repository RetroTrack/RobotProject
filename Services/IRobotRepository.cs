namespace RobotProject.Services;
public interface IRobotRepository
{
    void InsertRobot(User user);
    public List<User> GetAllRobots();

}