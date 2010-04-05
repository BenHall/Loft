using System.Collections.Generic;
using Xunit;

namespace Loft.Specs
{
    public class QueryResultSpecs
    {
        Server server;
        Database database;

        public QueryResultSpecs()
        {
            server = new Server(CouchDbTestServer.Server, CouchDbTestServer.Port);
            database = new Database(server, CouchDbTestServer.TestDb1);
        }

        [Fact]
        public void Get_As_TestDocument_should_pull_values_from_json()
        {
            QueryResult results = database.Query("specs", "query_design_document1");
            IList<TestDocument> documents = results.Get<TestDocument>();
            Assert.Equal(2, documents.Count);
        }

        [Fact]
        public void Get_As_TestDocument_should_have_test_value_property_populated()
        {
            QueryResult results = database.Query("specs", "query_design_document1");
            IList<TestDocument> documents = results.Get<TestDocument>();
            Assert.Equal("value", documents[0].Test);
            Assert.Equal("Document 2", documents[1].Test);
        }
    }
}