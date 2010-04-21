using System;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Loft.Specs
{
    public class JsonRequesterSpecs : IDisposable
    {
        FootReST.Server stubServer;

        public JsonRequesterSpecs()
        {
            stubServer = new FootReST.Server();
            stubServer.Start();
        }

        public void Dispose()
        {
            stubServer.Close();
        }

        [Fact]
        public void Can_send_GET_request_to_server()
        {
            stubServer.DefineCustomResponse("GET", "/", "{\"ok\": true}");   

            JsonRequester requester = new JsonRequester();
            JObject jObject = requester.Get(new Server("localhost", 5984), "/");
            Assert.True(jObject["ok"].Value<bool>());
        }

        [Fact]
        public void Can_send_PUT_request_to_server()
        {
            stubServer.DefineCustomResponse("PUT", "/", "{\"ok\": true}");

            JsonRequester requester = new JsonRequester();
            JObject jObject = requester.Put(new Server("localhost", 5984), "/", "");
            Assert.True(jObject["ok"].Value<bool>());
        }
    }
}