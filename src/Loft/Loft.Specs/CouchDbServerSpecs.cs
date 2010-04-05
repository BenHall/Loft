using Xunit;

namespace Loft.Specs
{
    public class ServerSpecs
    {
        [Fact]
        public void CanConnect_returns_true_if_server_is_available()
        {
            Server server = new Server(CouchDbTestServer.Server, CouchDbTestServer.Port);
            bool result = server.CanConnect();
            Assert.True(result);
        }

        [Fact]
        public void CanConnect_returns_false_if_server_is_available()
        {
            Server server = new Server("127.0.0.1", CouchDbTestServer.Port);
            bool result = server.CanConnect();
            Assert.False(result);
        }

        [Fact]
        public void GetVersion_returns_version_for_the_server()
        {
            //{"couchdb":"Welcome","version":"0.11.0"}
            Server server = new Server(CouchDbTestServer.Server, CouchDbTestServer.Port);
            string version = server.GetVersion();

            Assert.Equal("0.11.0", version);
        }
    }
}