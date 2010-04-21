namespace Loft
{
    public interface IDatabase
    {
        bool Exists();
        QueryResult Query(string design, string view);
        IRequester Requester { get; set; }
    }
}