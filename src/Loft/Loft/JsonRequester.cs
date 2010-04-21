using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;

namespace Loft
{
    public class JsonRequester : IRequester
    {
        public JObject Get(Server server, string endpoint)
        {
            Stream stream = MakeRequest(server, endpoint, "GET", string.Empty);
            return GetResponseJson(stream);
        }

        public JObject Put(Server server, string endpoint, string data)
        {
            Stream stream = MakeRequest(server, endpoint, "PUT", data);
            return GetResponseJson(stream);            
        }

        private JObject GetResponseJson(Stream responseStream)
        {
            string json;
            using (var reader = new StreamReader(responseStream))
            {
                json = reader.ReadToEnd();
            }

            return JObject.Parse(json);
        }

        private Stream MakeRequest(Server server, string endpoint, string verb, string data)
        {
            var request = (HttpWebRequest)WebRequest.Create(string.Format("http://{0}:{1}/{2}", server.Host, server.Port, endpoint));
            request.Method = verb; 
            var response = (HttpWebResponse)request.GetResponse();
            return response.GetResponseStream();
        }
    }
}