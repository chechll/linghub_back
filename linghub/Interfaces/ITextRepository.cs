﻿namespace linghub.Interfaces
{
    public interface ITextRepository
    {
        Text GetText(int id);
        ICollection<Text> GetAllText();
        bool isTextExist(int id);
        bool Save();
        bool CreateText(Text text);
        bool DeleteText(Text text);
        bool UpdateText(Text text);
        int GetUnsolvedTextId(int idUser);
        int GetSolvedTextCount(int idUser);
        int GetTextCount();
    }
}
