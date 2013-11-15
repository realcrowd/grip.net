using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;

namespace RealCrowd.PublishControl
{
    public abstract class Format
    {
        public abstract string Name { get; }

        public abstract object ToObject();
    }

    public class HttpResponseFormat : Format
    {
        public int Code { get; set; }

        public string Reason { get; set; }

        public List<KeyValuePair<string, string>> Headers { get; set; }

        public string Body { get; set; }

        public override string Name
        {
            get { return "http-response"; }
        }

        public override object ToObject()
        {
            var d = new Dictionary<string, object>();
            d["code"] = Code;
            d["reason"] = Reason;
            d["headers"] = Headers.Select(pair => new List<string>() { pair.Key, pair.Value });
            d["body"] = Body;
            return d;
        }
    }

    public class FppFormat : Format
    {
        public object Value { get; set; }

        public override string Name
        {
            get { return "fpp"; }
        }

        public override object ToObject()
        {
            return Value;
        }
    }

    public class Item
    {
        public List<Format> Formats { get; set; }

        public string Id { get; set; }

        public string PrevId { get; set; }

        public Dictionary<string, object> ToDictionary()
        {
            var d = new Dictionary<string, object>();
            if (Id != null)
                d["id"] = Id;
            if (PrevId != null)
                d["prev-id"] = PrevId;
            foreach (var format in Formats)
                d[format.Name] = format.ToObject();
            return d;
        }
    }

    public class PublishControl
    {
        private string baseUri;
        private string authBasicUser;
        private string authBasicPass;
        private Dictionary<string, object> authJwtClaim;
        private byte[] authJwtKey;

        public PublishControl(string baseUri)
        {
            this.baseUri = baseUri;
        }

        public void SetAuthBasic(string username, string password)
        {
            authBasicUser = username;
            authBasicPass = password;
        }

        public void SetAuthJwt(Dictionary<string, object> claim, byte[] key)
        {
            authJwtClaim = claim;
            authJwtKey = key;
        }

        public async Task PublishAsync(string channel, Item item)
        {
            var uri = baseUri + "/publish/";

            string auth = null;
            if (authBasicUser != null)
            {
                auth = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(authBasicUser + ":" + authBasicPass));
            }
            else if (authJwtClaim != null)
            {
                Dictionary<string, object> claim;
                if (!authJwtClaim.ContainsKey("exp"))
                {
                    claim = JsonConvert.DeserializeObject<Dictionary<string, object>>(JsonConvert.SerializeObject(authJwtClaim));
                    claim["exp"] = (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
                }
                else
                {
                    claim = authJwtClaim;
                }

    			auth = "Bearer " + JWT.JsonWebToken.Encode(claim, authJwtKey, JWT.JwtHashAlgorithm.HS256);
            }

            var i = item.ToDictionary();
            i["channel"] = channel;
            var content = new Dictionary<string, object>();
            content["items"] = new List<Dictionary<string, object>>() { i };
            HttpContent hc = new ByteArrayContent(Encoding.Default.GetBytes(JsonConvert.SerializeObject(content)));
            hc.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var httpClient = new HttpClient();
            if (auth != null)
            {
                httpClient.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(auth);
            }
            var response = await httpClient.PostAsync(uri, hc);
        }
    }
}
