using Newtonsoft.Json.Linq;

namespace Loft
{
    public interface IRequester
    {
        JObject Get(Server server, string endpoint);
        JObject Put(Server server, string endpoint, string data);
    }
}