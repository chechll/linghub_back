namespace linghub.Interfaces
{
    public interface IUserRepository
    {
        User GetUser(int id);
        ICollection<User> GetUsers();
        bool isUserExist(int id);
        bool logIn(int Id, string password);
        bool IsAdmin(int Id);
        bool Save();
        bool CreateUser(User user);
        bool DeleteUser(User user);
        bool UpdateUser(User user);
    }
}
