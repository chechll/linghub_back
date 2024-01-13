namespace linghub.Interfaces
{
    public interface IErrorRepository
    {
        Error GetError(int id);
        ICollection<Error> GetError();
        bool isErrorExist(int id);
        bool Save();
        bool CreateError(Error error);
        bool DeleteError(Error error);
        bool UpdateError(Error error);
    }
}
