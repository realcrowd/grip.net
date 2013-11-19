using Newtonsoft.Json;
using RealCrowd.PublishControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grip
{
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

    public class HttpStreamFormat : Format
    {
        public string Content { get; set; }

        public Boolean Close { get; set; }

        public override string Name
        {
            get { return "http-stream"; }
        }

        public override object ToObject()
        {
            var d = new Dictionary<string, object>();
            if (Close)
                d["action"] = "close";
            else
                d["content"] = Content;

            return d;
        }
    }

    public class Configuration
    {
        public class PublishControlConfiguration
        {
            public string ControlUri { get; set; }

            public string ControlIss { get; set; }

            public byte[] Key { get; set; }
        }

        public List<PublishControlConfiguration> PublishControls { get; set; }

        public Configuration(string config)
        {
            var pubList = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(config);
            foreach (var pubConfig in pubList)
            {
                var controlUri = pubConfig["controlUri"].ToString();
                string controlIss = null;
                byte[] key = null;
                if (pubConfig.ContainsKey("controlIss"))
                {
                    controlIss = pubConfig["controlIss"].ToString();
                    var keyStr = pubConfig["key"].ToString();
                    if (keyStr.StartsWith("base64:"))
                        key = Convert.FromBase64String(keyStr.Substring(7));
                    else
                        key = Encoding.UTF8.GetBytes(keyStr);
                }

                PublishControls.Add(new PublishControlConfiguration() { ControlUri = controlUri, ControlIss = controlIss, Key = key });
            }
        }
    }

    public class GripPublishControl
    {
        private List<PublishControl> publishControls;

        public void Clear()
        {
            publishControls.Clear();
        }

        public void Add(PublishControl publishControl)
        {
            publishControls.Add(publishControl);
        }

        public void ApplyConfigurationString(string configString)
        {
            var config = new Configuration(configString);

            foreach (var publishControlConfig in config.PublishControls)
            {
                var publishControl = new PublishControl(publishControlConfig.ControlUri);
                if (publishControlConfig.ControlIss != null)
                {
                    var claim = new Dictionary<string, object>();
                    claim["iss"] = publishControlConfig.ControlIss;
                    publishControl.SetAuthJwt(claim, publishControlConfig.Key);
                }

                publishControls.Add(publishControl);
            }
        }

        public async Task PublishAsync(string channel, string id, string prevId, HttpResponseFormat responseFormat = null, HttpStreamFormat streamFormat = null)
        {
            var item = new Item() { Id = id, PrevId = prevId };

            if (responseFormat != null)
                item.Formats.Add(responseFormat);
            if (streamFormat != null)
                item.Formats.Add(streamFormat);

            var tasks = new List<Task>();
            foreach (var pub in publishControls)
                tasks.Add(pub.PublishAsync(channel, item));

            await Task.WhenAll(tasks);
        }
    }
}
