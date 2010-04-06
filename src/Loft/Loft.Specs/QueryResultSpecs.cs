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

        [Fact]
        public void Get_As_TestDocument_should_only_return_type_specified()
        {
            QueryResult results = database.Query("specs", "query_design_document_multiple");
            IList<TestDocument> testDocuments = results.Get<TestDocument>("QueryDesignDocumentSpec");
            Assert.Equal("value", testDocuments[0].Test);
            Assert.Equal("Document 2", testDocuments[1].Test);

            IList<TestDocument> test2Documents = results.Get<TestDocument>("QueryDesignDocumentSpec2");
            Assert.Equal("Document 3", test2Documents[0].Test);
            Assert.Equal("value 2", test2Documents[1].Test);
        }
    }
}