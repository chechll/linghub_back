using linghub.Data;
using linghub.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace linghub.Repository
{
    public class TextRepository : ITextRepository
    {
        private readonly LinghubContext _context;

        public TextRepository(LinghubContext context)
        {
            _context = context;
        }

        public bool CreateText(Text text)
        {
            _context.Add(text);

            return Save();
        }

        public bool DeleteText(Text text)
        {
            _context.Remove(text);

            return Save();
        }

        public ICollection<Text> GetAllText()
        {
            return _context.Texts.OrderBy(p => p.IdText).ToList();
        }

        public Text GetText(int id)
        {
            return _context.Texts.Where(p => p.IdText == id).FirstOrDefault();
        }

        public int GetSolvedTextCount(int idUser)
        {
            return _context.UTexts.Count(uText => uText.IdUser == idUser);
        }

        public int GetTextCount()
        {
            return _context.Texts.Count();
        }

        public int GetUnsolvedTextId(int idUser)
        {
            var unsolvedTextId = _context.Texts
            .Where(text => !_context.UTexts.Any(uText => uText.IdText == text.IdText && uText.IdUser == idUser))
            .Select(text => text.IdText)
            .FirstOrDefault();

            return unsolvedTextId;
        }

        public bool isTextExist(int id)
        {
            return _context.Texts.Any(p => p.IdText == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateText(Text text)
        {
            _context.Update(text);
            return Save();
        }
    }
}
