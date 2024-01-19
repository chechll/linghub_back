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

        public bool DeleteUTexts(List<UText> uTexts)
        {
            _context.RemoveRange(uTexts);

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

        public ICollection<UText> GetUTextsToDeleteByTextId(int idText)
        {
            return _context.UTexts.Where(p => p.IdText == idText).ToList();
        }

        public ICollection<UText> GetUTextsToDeleteByUserId(int idUser)
        {
            return _context.UTexts.Where(p => p.IdUser == idUser).ToList();
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
