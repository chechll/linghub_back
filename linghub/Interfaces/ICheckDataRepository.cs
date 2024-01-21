using linghub.Dto;

namespace linghub.Interfaces
{
    public interface ICheckDataRepository 
    {
        bool IsValidEmail(string email);
        bool CheckStringLengs(string word, int length);
        bool IsEnglishWord(string input);
        bool IsUkranianWord(string input);
        bool IsEnglishSentence(string input);
        bool IsUkranianSentence(string input);
        string checkUser(UserDto user);
        string checkText(TextDto text);
        string checkWord(WordDto word);
    }
}
