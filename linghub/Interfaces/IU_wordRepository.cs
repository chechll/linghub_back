namespace linghub.Interfaces
{
    public interface IU_wordRepository
    {
        UWord GetUWord(int id);
        ICollection<UWord> GetUWords();
        bool isUwordExist(int id);
        bool Save();
        bool CreateUword(UWord uWord);
        bool DeleteUword(UWord uWord);
        bool UpdateUword(UWord uWord);
    }
}
