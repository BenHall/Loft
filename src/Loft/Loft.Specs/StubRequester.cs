using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Loft.Specs
{
    class StubRequester : IRequester
    {
        Dictionary<string, JObject> Dictionary = new Dictionary<string, JObject>();
        public void ReturnThis(string endpoint, string result)
        {
            Dictionary.Add(endpoint, JObject.Parse(result));
        }

        public JObject Get(Server server, string endpoint)
        {
            if(Dictionary.ContainsKey(endpoint))
                return Dictionary[endpoint];

            throw new MissingFieldException("Missing " + endpoint);
        }

        public JObject Put(Server server, string endpoint, string data)
        {
            return Dictionary[endpoint];
        }
    }
}