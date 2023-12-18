namespace linghub.Interfaces
{
    public interface IWordRepository
    {
        Word GetWord(int id);
        ICollection<Word> GetWords();
        bool isWordExist(int id);
        bool Save();
        bool CreateWord(Word word);
        bool DeleteWord(Word word);
        bool UpdateWord(Word word);
    }
}
