namespace linghub.Interfaces
{
    public interface IUserRepository
    {
        User GetUser(int id);
        ICollection<User> GetUsers();
        bool isUserExist(int id);
        int GetId(string email);
        bool IsAdmin(int Id);
        bool Save();
        bool CreateUser(User user);
        bool DeleteUser(User user);
        bool UpdateUser(User user);
    }
}
