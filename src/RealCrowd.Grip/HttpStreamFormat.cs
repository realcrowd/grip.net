using RealCrowd.PublishControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealCrowd.Grip
{
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
}
