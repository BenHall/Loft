using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Loft.Specs
{
    public class StubRequester : IRequester
    {
        Dictionary<string, JContainer> Objects = new Dictionary<string, JContainer>();
        public void ReturnThis(string endpoint, string result)
        {
            if (Objects.ContainsKey(endpoint))
                Objects.Remove(endpoint);

            JContainer value = null;
            if (result.StartsWith("["))
                value = JArray.Parse(result);
            else
                value = JObject.Parse(result);

            Objects.Add(endpoint, value);
        }

        public JContainer Get(Server server, string endpoint)
        {
            if(Objects.ContainsKey(endpoint))
                return Objects[endpoint];

            throw new MissingFieldException("Missing " + endpoint);
        }

        public JContainer Put(Server server, string endpoint, string data)
        {
            return Objects[endpoint];
        }
    }
}