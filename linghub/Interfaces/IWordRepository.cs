namespace linghub.Interfaces
{
    public interface IWordRepository
    {
        Word GetWord(int id);
        bool isWordExist(int id);
        bool Save();
        bool CreateWord(Word word);
        bool DeleteWord(Word word);
        bool UpdateWord(Word word);
    }
}
