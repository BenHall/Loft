using Xunit;

namespace Loft.Specs
{
    public class DatabaseSpecs
    {
        Server server;
        Database database;

        public DatabaseSpecs()
        {
            server = new Server(CouchDbTestServer.Server, CouchDbTestServer.Port);
            database = new Database(server, CouchDbTestServer.TestDb1);
        }

        [Fact]
        public void Exists_returns_true_if_the_database_exists()
        {
            
            bool result = database.Exists();
            Assert.True(result);
        }

        [Fact]
        public void QueryDesignDocument_returns_total_rows_returned_by_view()
        {
            //curl -vX GET http://192.168.1.66:5984/testdb1/_design/specs/_view/query_design_document1
            //{"total_rows":2,"offset":0,"rows":[
            //{"id":"1f2dfc89e8df7092bd92b31be3000cf0","key":null,"value":{"_id":"1f2dfc89e8df7092bd92b31be3000cf0","_rev":"1-71112dee2e9ce973e92ec27c5ef3c33e","test":"value","type":"QueryDesignDocumentSpec"}},
            //{"id":"1f2dfc89e8df7092bd92b31be30011cd","key":null,"value":{"_id":"1f2dfc89e8df7092bd92b31be30011cd","_rev":"1-a6414af42c97006e63044bb6872c769c","test":"Document 2","type":"QueryDesignDocumentSpec"}}
            //]}

            QueryResult results = database.Query("specs", "query_design_document1");
            Assert.Equal(2, results.TotalRows);
        }

        [Fact]
        public void QueryDesignDocument_returns_two_items()
        {
            QueryResult results = database.Query("specs", "query_design_document1");
            Assert.Equal(2, results.Items.Count);
        }
    }
}