using linghub.Data;
using linghub.Interfaces;
using linghub.Models;

namespace linghub.Repository
{
    public class ErrorRepository : IErrorRepository
    {
        private readonly LinghubContext _context;

        public ErrorRepository(LinghubContext context)
        {
            _context = context;
        }
        public bool CreateError(Error error)
        {
            _context.Add(error);

            return Save();
        }

        public bool DeleteError(Error error)
        {
            _context.Remove(error);

            return Save();
        }

        public Error GetError(int id)
        {
            return _context.Errors.Where(p => p.Id == id).FirstOrDefault();
        }

        public ICollection<Error> GetError()
        {
            return _context.Errors.OrderBy(p => p.Id).ToList();
        }

        public bool isErrorExist(int id)
        {
            return _context.Errors.Any(p => p.Id == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateError(Error error)
        {
            _context.Update(error);
            return Save();
        }
    }
}
