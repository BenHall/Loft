using System;
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
    }
}