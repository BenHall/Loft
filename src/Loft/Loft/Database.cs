using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Loft
{
    public class Database : IDatabase
    {
        private readonly Server _server;
        private readonly string _databaseName;
        private IRequester _requester;

        public IRequester Requester
        {
            get {
                if(_requester == null)
                    _requester = new JsonRequester();
                return _requester;
            }
            set {
                _requester = value;
            }
        }

        public Database(Server server, string databaseName)
        {
            _server = server;
            _databaseName = databaseName;
        }

        public bool Exists()
        {
            JObject jObject = Requester.Get(_server, _databaseName);
            return jObject.Value<string>("error") != "not_found";
        }

        public QueryResult Query(string design, string view)
        {
            JObject jObject = Requester.Get(_server, string.Format("{0}/_design/{1}/_view/{2}", _databaseName, design, view));

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

        public T Save<T>(T document)
        {
            document.SetValue("Id", GenerateID());
            JObject result = Requester.Put(_server, document.GetValue("Id"), JsonConvert.SerializeObject(document));
            if(result["ok"].Value<bool>())
                document.SetValue("Rev", result["rev"].Value<string>());
            return document;
        }

        public string GenerateID()
        {
            return Requester.Get(_server, "_uuids")["uuids"].Value<JArray>()[0].Value<string>();
        }
    }
}