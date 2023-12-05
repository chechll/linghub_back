namespace linghub.Interfaces
{
    public interface IUserRepository
    {
        User GetUser(int id);
        bool isUserExist(int id);
        bool Save();
        bool CreateUser(User user);
        bool DeleteUser(User user);
        bool UpdateUser(User user);
    }
}
