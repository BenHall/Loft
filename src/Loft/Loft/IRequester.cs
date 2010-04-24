using Newtonsoft.Json.Linq;

namespace Loft
{
    public interface IRequester
    {
        JContainer Get(Server server, string endpoint);
        JContainer Put(Server server, string endpoint, string data);
    }
}