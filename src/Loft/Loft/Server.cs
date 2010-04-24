using System.Net.Sockets;
using Newtonsoft.Json.Linq;

namespace Loft
{
    public interface IServer
    {
        bool CanConnect();
    }

    public class Server : IServer
    {
        private readonly string _host;
        private readonly int _port;

        public string Host
        {
            get { return _host; }
        }

        public int Port
        {
            get { return _port; }
        }

        public Server(string host, int port)
        {
            _host = host;
            _port = port;
        }

        public bool CanConnect()
        {
            using (var c = new TcpClient())
            {
                try
                {
                    c.Connect(Host, Port);
                    return c.Connected;
                }
                catch (SocketException)
                {
                    return false;
                }
            }
        }

        public string GetVersion()
        {
            JsonRequester requester = new JsonRequester();
            JContainer json = requester.Get(this, "");

            return json.Value<string>("version");
        }
    }
}