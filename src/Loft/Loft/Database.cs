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
            JContainer jObject = Requester.Get(_server, _databaseName);
            return jObject.Value<string>("error") != "not_found";
        }

        public QueryResult Query(string design, string view)
        {
            JContainer jObject = Requester.Get(_server, string.Format("{0}/_design/{1}/_view/{2}", _databaseName, design, view));

            QueryResult result = new QueryResult();
            result.TotalRows = jObject.Value<int>("total_rows");
            result.Offset = jObject.Value<int>("offset");

            result.Items = GetItemsFromJsonRows(jObject);

            return result;
        }

        private IList<ResultItem> GetItemsFromJsonRows(JContainer results)
        {
            var items = new List<ResultItem>();

            JArray rows = results["rows"].Value<JArray>();
            foreach (JToken row in rows)
            {
                items.Add(new ResultItem { Id = row.Value<string>("id"), Key = row.Value<string>("key"), Value = row["value"].ToString() });
            }

            return items;
        }

        public T Save<T>(T document)
        {
            GetOrSetID(document);

            JContainer result = Requester.Put(_server, _databaseName + "/" + document.GetValue("Id"), ConvertToJson(document));
            if(result["ok"].Value<bool>())
                SetRev(document, result);

            return document;
        }

        private void GetOrSetID<T>(T document)
        {
            string documentId = document.GetValue("Id");
            if(string.IsNullOrEmpty(documentId))
                document.SetValue("Id", GenerateID());
        }

        public List<T> Save<T>(List<T> document)
        {
            List<string> convertedDocuments = new List<string>();

            foreach (var doc in document)
            {
                GetOrSetID(doc);
                convertedDocuments.Add(ConvertToJson(doc));
            }

            string joinedDocs = string.Join(",", convertedDocuments.ToArray());
            string jsonRequest = "{\"docs\": [" + joinedDocs + "]}";

            JContainer result = Requester.Put(_server, _databaseName + "/_bulk_docs", jsonRequest);
            
            for (int index = 0; index < result.Value<JArray>().Count; index++)
            {
                var rev = result.Value<JArray>()[index]["rev"].Value<string>();
                T originalDoc = document[index];

                originalDoc.SetValue("Rev", rev);
            }

            return document;
        }

        private void SetRev<T>(T document, JContainer result)
        {
            document.SetValue("Rev", result["rev"].Value<string>());
        }

        private string ConvertToJson<T>(T document)
        {
            var result = JsonConvert.SerializeObject(document);
            return result.Replace("\"rev\":", "\"_rev\":");
        }

        public string GenerateID()
        {
            return Requester.Get(_server, "_uuids")["uuids"].Value<JArray>()[0].Value<string>();
        }

        public QueryResult Query(string design, string view, Dictionary<string, string> parameters)
        {
            string parametersAsString = ConvertDictionaryToParameters(parameters);
            string viewWithParameters = view + "?" + parametersAsString;
            return Query(design, viewWithParameters);
        }

        private string ConvertDictionaryToParameters(Dictionary<string, string> parameters)
        {
            List<string> builder = new List<string>();
            foreach (KeyValuePair<string, string> keyValuePair in parameters)
            {
                builder.Add(keyValuePair.Key + "=\"" + keyValuePair.Value + "\"");
            }

            return string.Join("&", builder.ToArray());
        }
    }
}