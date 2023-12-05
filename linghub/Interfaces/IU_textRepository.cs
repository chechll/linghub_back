namespace linghub.Interfaces
{
    public interface IU_textRepository
    {
        UText GetUText(int id);
        bool isUtextExist(int id);
        bool Save();
        bool CreateUText(UText uText);
        bool DeleteUText(UText uText);
        bool UpdateUText(UText uText);
    }
}
