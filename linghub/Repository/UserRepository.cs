using linghub.Data;
using linghub.Interfaces;

namespace linghub.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly LinghubContext _context;

        public UserRepository(LinghubContext context)
        {
            _context = context;
        }

        public bool CreateUser(User user)
        {
            _context.Add(user);

            return Save();
        }

        public bool DeleteUser(User user)
        {
            _context.Remove(user);

            return Save();
        }

        public User GetUser(int id)
        {
            return _context.Users.Where(p => p.IdUser == id).FirstOrDefault();
        }

        public ICollection<User> GetUsers()
        {
            return _context.Users.OrderBy(p => p.IdUser).ToList();
        }

        public bool IsAdmin(int Id)
        {
            if(_context.Users.Where(p => p.IdUser == Id).Select(c => c.Admin).FirstOrDefault() == 1) return true;
            return false;
        }

        public bool isUserExist(int id)
        {
            return _context.Users.Any(p => p.IdUser == id);
        }

        public int GetId(string email)
        {
            int userId = _context.Users
            .Where(p => p.Email == email)
            .Select(p => p.IdUser)
            .FirstOrDefault();

            return userId;
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateUser(User user)
        {
            var existingUser = _context.Users.Find(user.IdUser);

            if (existingUser == null)
            {
                return false;
            }

            _context.Entry(existingUser).CurrentValues.SetValues(user);

            return Save();
        }
    }
}
