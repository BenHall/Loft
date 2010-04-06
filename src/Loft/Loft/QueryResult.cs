using System.Collections.Generic;
using Newtonsoft.Json;

namespace Loft
{
    public class QueryResult
    {
        public int TotalRows { get; set; }
        public int Offset { get; set; }

        public IList<ResultItem> Items { get; set; }

        public IList<T> Get<T>()
        {
            return Get<T>(string.Empty);
        }

        public IList<T> Get<T>(string type)
        {
            var list = new List<T>();
            foreach (var resultItem in Items)
            {
                if (string.IsNullOrEmpty(type) || resultItem.Value.Contains("\"type\": \"" + type + "\""))
                {
                    string cleaned = CleanJson(resultItem.Value);
                    list.Add(JsonConvert.DeserializeObject<T>(cleaned));
                }
            }

            return list;
        }

        private string CleanJson(string value)
        {
            return value.Replace("_id", "id").Replace("_rev", "rev");
        }
    }
}