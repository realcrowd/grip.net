using RealCrowd.PublishControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealCrowd.Grip
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
}
