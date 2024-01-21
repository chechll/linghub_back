using linghub.Data;
using linghub.Dto;
using linghub.Interfaces;
using linghub.Models;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace linghub.Repository
{
    public class CheckDataRepository : ICheckDataRepository
    {
        private readonly Regex _englishWordRegex;
        private readonly Regex _ukranianWordRegex;
        private readonly Regex _englishSentenceRegex;
        private readonly Regex _ukranianSentenceRegex;

        public CheckDataRepository()
        {
            _englishWordRegex = new Regex("^[a-zA-Z]+$");
            _ukranianWordRegex = new Regex("^[а-яА-ЯіІїЇєЄ]+$");
            _englishSentenceRegex = new Regex("^[a-zA-Z0-9 .,!?:;-«»\"'()-]+$"); 
            _ukranianSentenceRegex = new Regex("^[а-яА-ЯіІїЇєЄґҐ0-9 .,!?:;-«»\"'()-]+$");
        }

        public bool CheckStringLengs(string word, int length)
        {
            if (word.Length <= length) { return true; }
            return false;
        }

        public bool IsValidEmail(string email)
        {
            try
            {
                var emailAddress = new MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool IsEnglishWord(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;
            return _englishWordRegex.IsMatch(input);
        }

        public bool IsUkranianWord(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;
            return _ukranianWordRegex.IsMatch(input);
        }

        public bool IsEnglishSentence(string input)
        {
            return _englishSentenceRegex.IsMatch(input);
        }

        public bool IsUkranianSentence(string input)
        {
            return _ukranianSentenceRegex.IsMatch(input);
        }

        public string checkUser(UserDto user)
        {

            string response = "Ok";
            if (!IsValidEmail(user.Email))
            {
                response = "your mail is invalid";
                return response;
            }

            if (!CheckStringLengs(user.Email, 30))
            {
                response = "your mail length is more then 30";
                return response;
            }

            if (!CheckStringLengs(user.UserPassword, 30))
            {
                response = "your password length is more then 30";
                return response;
            }

            if (!CheckStringLengs(user.Name, 15) || !CheckStringLengs(user.Surname, 15))
            {
                response = "your name or surname is more then 15";
                return response;
            }

            if (user.UserPassword == null)
            {
                response = "wrong data";
                return response;
            }

            return response;
        }

        public string checkText(TextDto text)
        {
            string response = "Ok";

            if (!IsUkranianSentence(text.Ans) || !CheckStringLengs(text.Ans, 15) || 
                !IsUkranianSentence(text.Ans1) || !CheckStringLengs(text.Ans1, 15) || 
                !IsUkranianSentence(text.Ans2) || !CheckStringLengs(text.Ans2, 15)|| 
                !IsUkranianSentence(text.Ans3) || !CheckStringLengs(text.Ans3, 15))
            {
                response = "problem with some ans. check if they are in ukranian and less than 15 symbols";
                return response;
            }

            if (!IsUkranianSentence(text.Question) || !CheckStringLengs(text.Question, 30))
            {
                response = "problem with question. check if it's in ukranian and less than 30 symbols";
                return response;
            }

            if (!IsUkranianSentence(text.TextName) || !CheckStringLengs(text.TextName, 15))
            {
                response = "problem with name. check if it's in ukranian and less than 15 symbols";
                return response;
            }

            if (!IsUkranianSentence(text.Text1))
            {
                response = "problem with text. check if it's in ukranian";
                return response;
            }

            if (text.IdAns > 4 || text.IdAns < 1)
            {
                response = "problem with idAns. check if it's in 1-4";
                return response;
            }

            return response;
        }

        public string checkWord(WordDto word)
        {
            string response = "Ok";

            if (!IsUkranianWord(word.Uaword) || !CheckStringLengs(word.Uaword, 15)) {
                response = "problem with uaWord. Check if it's in Ukrainian and less than 15 symbols";
                return response;
            }

            if (!IsEnglishWord(word.Enword) || !CheckStringLengs(word.Enword, 15))
            {
                response = "problem with enWord. Check if it's in English and less than 15 symbols";
                return response;
            }

            if (!IsUkranianSentence(word.Uasent) || !CheckStringLengs(word.Uasent, 255))
            {
                response = "problem with uasent. check if it's in ukranian and less than 255 symbols";
                return response;
            }

            if (!IsEnglishSentence(word.Ensent) || !CheckStringLengs(word.Ensent, 255))
            {
                response = "problem with ensent. check if it's in english and less than 255 symbols";
                return response;
            }

            return response;
        }
    }
}
