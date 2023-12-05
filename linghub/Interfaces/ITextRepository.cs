namespace linghub.Interfaces
{
    public interface ITextRepository
    {
        Text GetText(int id);
        bool isTextExist(int id);
        bool Save();
        bool CreateText(Text text);
        bool DeleteText(Text text);
        bool UpdateText(Text text);
    }
}
