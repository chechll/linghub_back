using linghub.Data;
using linghub.Interfaces;

namespace linghub.Repository
{
    public class UtextRepository : IU_textRepository
    {
        private readonly LinghubContext _context;

        public UtextRepository(LinghubContext context)
        {
            _context = context;
        }

        public bool CreateUText(UText uText)
        {
            _context.Add(uText);

            return Save();
        }

        public bool DeleteUText(UText uText)
        {
            _context.Remove(uText);

            return Save();
        }

        public UText GetUText(int id)
        {
            return _context.UTexts.Where(p => p.Id == id).FirstOrDefault();
        }

        public ICollection<UText> GetUTexts()
        {
            return _context.UTexts.OrderBy(p => p.Id).ToList();
        }

        public bool isUtextExist(int id)
        {
            return _context.UTexts.Any(p => p.Id == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateUText(UText uText)
        {
            _context.Update(uText);
            return Save();
        }
    }
}
