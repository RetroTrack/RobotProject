namespace RobotProject.Services;
public interface IUserRepository
{
    void InsertUser(User user);
    public List<User> GetAllUsers();

}