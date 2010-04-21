using System;
using Xunit;

namespace Loft.Specs
{
    public class DatabaseSpecs
    {
        Server server;
        Database database;
        StubRequester requester;

        public DatabaseSpecs()
        {
            requester = new StubRequester();
            server = new Server(CouchDbTestServer.Server, CouchDbTestServer.Port);
            database = new Database(server, CouchDbTestServer.TestDb1);
            database.Requester = requester;
        }

        [Fact]
        public void Exists_returns_true_if_the_database_exists()
        {
            requester.ReturnThis("testdb1", "{\"error\":\"no\"}");

            bool result = database.Exists();
            Assert.True(result);
        }

        [Fact]
        public void QueryDesignDocument_returns_total_rows_returned_by_view()
        {
            requester.ReturnThis("testdb1/_design/specs/_view/query_design_document1", "{\"total_rows\":2,\"offset\":0,\"rows\":[{\"id\":\"1f2dfc89e8df7092bd92b31be3000cf0\",\"key\":null,\"value\":{\"_id\":\"1f2dfc89e8df7092bd92b31be3000cf0\",\"_rev\":\"1-71112dee2e9ce973e92ec27c5ef3c33e\",\"test\":\"value\",\"type\":\"QueryDesignDocumentSpec\"}},{\"id\":\"1f2dfc89e8df7092bd92b31be30011cd\",\"key\":null,\"value\":{\"_id\":\"1f2dfc89e8df7092bd92b31be30011cd\",\"_rev\":\"1-a6414af42c97006e63044bb6872c769c\",\"test\":\"Document 2\",\"type\":\"QueryDesignDocumentSpec\"}}]}");

            QueryResult results = database.Query("specs", "query_design_document1");
            Assert.Equal(2, results.TotalRows);
        }

        [Fact]
        public void QueryDesignDocument_returns_two_items()
        {
            requester.ReturnThis("testdb1/_design/specs/_view/query_design_document1", "{\"total_rows\":2,\"offset\":0,\"rows\":[{\"id\":\"1f2dfc89e8df7092bd92b31be3000cf0\",\"key\":null,\"value\":{\"_id\":\"1f2dfc89e8df7092bd92b31be3000cf0\",\"_rev\":\"1-71112dee2e9ce973e92ec27c5ef3c33e\",\"test\":\"value\",\"type\":\"QueryDesignDocumentSpec\"}},{\"id\":\"1f2dfc89e8df7092bd92b31be30011cd\",\"key\":null,\"value\":{\"_id\":\"1f2dfc89e8df7092bd92b31be30011cd\",\"_rev\":\"1-a6414af42c97006e63044bb6872c769c\",\"test\":\"Document 2\",\"type\":\"QueryDesignDocumentSpec\"}}]}");

            QueryResult results = database.Query("specs", "query_design_document1");
            Assert.Equal(2, results.Items.Count);
        }

        [Fact]
        public void Save_stores_document_on_the_server()
        {
            //http://wiki.apache.org/couchdb/HTTP_Document_API

            requester.ReturnThis("_uuids", "{\"uuids\":[\"" + CouchDbTestServer.DocumentId1 + "\"]}");
            requester.ReturnThis(CouchDbTestServer.DocumentId1, "{\"ok\":true,\"id\":\"" +CouchDbTestServer.DocumentId1+ "\",\"rev\":\"1-5d978e67df51d48cc5876a09053ee342\"}");

            TestDocument document = new TestDocument { Test = new Random().Next().ToString() };
            TestDocument savedDocument = database.Save(document);

            Assert.True(savedDocument.Rev.StartsWith("1"));
            Assert.Equal(CouchDbTestServer.DocumentId1, savedDocument.Id);
        }

        [Fact(Skip = "Pending")]
        public void Save_stores_a_collection_of_objects_on_server()
        {
            //http://wiki.apache.org/couchdb/HTTP_Bulk_Document_API
        }

        [Fact(Skip = "Pending")]
        public void Save_updates_document_if_it_has_a_rev()
        {
            //http://wiki.apache.org/couchdb/HTTP_Bulk_Document_API
        }

        [Fact]
        public void GetID_returns_a_new_ID_from_server()
        {
            //curl -X GET http://127.0.0.1:5984/_uuids
            requester.ReturnThis("_uuids", "{\"uuids\":[\"07d02e64f0e5347bc96152b77f0024b5\"]}");
            string id = database.GenerateID();

            Assert.Equal("07d02e64f0e5347bc96152b77f0024b5", id);
        }
    }
}