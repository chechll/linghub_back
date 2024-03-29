﻿using linghub.Data;
using linghub.Interfaces;

namespace linghub.Repository
{
    public class U_wordRepository : IU_wordRepository
    {
        private readonly LinghubContext _context;

        public U_wordRepository(LinghubContext context)
        {
            _context = context;
        }

        public bool CreateUword(UWord uWord)
        {
            _context.Add(uWord);

            return Save();
        }

        public bool DeleteUword(UWord uWord)
        {
            _context.Remove(uWord);

            return Save();
        }

        public bool DeleteUWords(List<UWord> uWords)
        {
            _context.RemoveRange(uWords);

            return Save();
        }

        public UWord GetUWord(int id)
        {
            return _context.UWords.Where(p => p.Id == id).FirstOrDefault();
        }

        public ICollection<UWord> GetUWords()
        {
            return _context.UWords.OrderBy(p => p.Id).ToList();
        }

        public ICollection<UWord> GetUwordsToDeleteByUserId(int idUser)
        {
            return _context.UWords.Where(p => p.IdUser == idUser).ToList();
        }

        public ICollection<UWord> GetUwordsToDeleteByWordId(int idWord)
        {
            return _context.UWords.Where(p => p.IdWord == idWord).ToList();
        }

        public bool isUwordExist(int id)
        {
            return _context.UWords.Any(p => p.Id == id);
        }

        public bool Save()
        {
            try
            {
                var saved = _context.SaveChanges();
                return saved > 0 ? true : false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
}

        public bool UpdateUword(UWord uWord)
        {
            _context.Update(uWord);
            return Save();
        }
    }
}
