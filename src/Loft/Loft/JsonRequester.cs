using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;

namespace Loft
{
    public class JsonRequester
    {
        public JObject Get(Server server, string endpoint)
        {
            Stream responseStream = MakeRequest(server, endpoint);
            
            string json;
            using (var reader = new StreamReader(responseStream))
            {
                json = reader.ReadToEnd();
            }

            return JObject.Parse(json);
        }

        private Stream MakeRequest(Server server, string endpoint)
        {
            var request = (HttpWebRequest)WebRequest.Create(string.Format("http://{0}:{1}/{2}", server.Host, server.Port, endpoint));
            var response = (HttpWebResponse)request.GetResponse();
            return response.GetResponseStream();
        }
    }
}