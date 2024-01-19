using linghub.Data;
using linghub.Interfaces;

namespace linghub.Repository
{
    public class WordRepository : IWordRepository
    {
        private readonly LinghubContext _context;

        public WordRepository(LinghubContext context)
        {
            _context = context;
        }

        public bool CreateWord(Word word)
        {
            _context.Add(word);

            return Save();
        }

        public bool DeleteWord(Word word)
        {
            _context.Remove(word);

            return Save();
        }

        public int GetSolvedWordCount(int idUser)
        {
            return _context.UWords.Count(uWord => uWord.IdUser == idUser);
        }

        public int GetWordCount()
        {
            return _context.Words.Count();
        }

        public int GetUnsolvedWordId(int idUser)
        {
            var unsolvedWordId = _context.Words
            .Where(word => !_context.UWords.Any(uWord => uWord.IdWord == word.IdWord && uWord.IdUser == idUser))
            .Select(word => word.IdWord)
            .FirstOrDefault();

            return unsolvedWordId;
        }

        public Word GetWord(int id)
        {
            return _context.Words.Where(p => p.IdWord == id).FirstOrDefault();
        }

        public ICollection<Word> GetAllWords()
        {
            return _context.Words.OrderBy(p => p.IdWord).ToList();
        }

        public bool isWordExist(int id)
        {
            return _context.Words.Any(p => p.IdWord == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateWord(Word word)
        {
            _context.Update(word);
            return Save();
        }
    }
}
