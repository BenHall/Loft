using Xunit;

namespace Loft.Specs
{
    public class DatabaseSpecs
    {
        [Fact]
        public void Exists_returns_true_if_the_database_exists()
        {
            Server server = new Server(CouchDbTestServer.Server, CouchDbTestServer.Port);
            Database database = new Database(server, CouchDbTestServer.TestDb1);
            bool result = database.Exists();
            Assert.True(result);
        }
    }
}