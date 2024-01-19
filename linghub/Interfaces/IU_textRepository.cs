namespace linghub.Interfaces
{
    public interface IU_textRepository
    {
        UText GetUText(int id);
        ICollection<UText> GetUTexts();
        bool isUtextExist(int id);
        bool Save();
        bool CreateUText(UText uText);
        bool DeleteUText(UText uText);
        bool UpdateUText(UText uText);
        ICollection<UText> GetUTextsToDeleteByUserId(int idUser);
        ICollection<UText> GetUTextsToDeleteByTextId(int idText);
        bool DeleteUTexts(List<UText> uTexts);
    }
}
