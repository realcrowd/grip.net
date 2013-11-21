using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealCrowd.PublishControl
{
    public class Configuration
    {
        public class Entry
        {
            public string ControlUri { get; set; }
            public string ControlIss { get; set; }
            public byte[] Key { get; set; }
        }

        public List<Entry> Entries { get; set; }

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

                Entries.Add(new Entry() { ControlUri = controlUri, ControlIss = controlIss, Key = key });
            }
        }
    }
}
