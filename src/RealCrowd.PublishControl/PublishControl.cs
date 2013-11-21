// Copyright (c) RealCrowd, Inc. All rights reserved. See LICENSE in the project root for license information.

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
