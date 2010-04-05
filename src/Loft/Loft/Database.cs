using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Loft
{
    public interface IDatabase
    {
        bool Exists();
    }

    public class Database : IDatabase
    {
        private readonly Server _server;
        private readonly string _databaseName;

        public Database(Server server, string databaseName)
        {
            _server = server;
            _databaseName = databaseName;
        }

        public bool Exists()
        {
            JsonRequester requester = new JsonRequester();
            JObject jObject = requester.Get(_server, _databaseName);
            return jObject.Value<string>("error") != "not_found";
        }

        public QueryResult Query(string design, string view)
        {
            JsonRequester requester = new JsonRequester();
            JObject jObject = requester.Get(_server, string.Format("{0}/_design/{1}/_view/{2}", _databaseName, design, view));

            QueryResult result = new QueryResult();
            result.TotalRows = jObject.Value<int>("total_rows");
            result.Offset = jObject.Value<int>("offset");

            result.Items = new List<ResultItem>();

            JArray rows = jObject["rows"].Value<JArray>();
            foreach (JToken row in rows)
            {
                result.Items.Add(new ResultItem { Id = row.Value<string>("id"), Key = row.Value<string>("key"), Value = row["value"].ToString()});
            }

            return result;
        }
    }
}