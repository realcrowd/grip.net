// Copyright (c) RealCrowd, Inc. All rights reserved. See LICENSE in the project root for license information.

using RealCrowd.PublishControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealCrowd.Grip
{
    public class Response
    {
        public int Code { get; set; }
        public string Reason { get; set; }
        public IList<KeyValuePair<string, string>> Headers { get; set; }
        public byte[] Body { get; set; }

        public Response()
        {
        }

        public object ToObject()
        {
            var d = new Dictionary<string, object>();

            if (Code > 0)
                d["code"] = Code;

            if (Reason != null)
                d["reason"] = Reason;

            if (Headers != null)
                d["headers"] = Headers.Select(pair => new List<string>() { pair.Key, pair.Value });

            if (Body != null)
            {
                try
                {
                    d["body"] = Encoding.UTF8.GetString(Body);
                }
                catch (ArgumentException)
                {
                    d["body-bin"] = Convert.ToBase64String(Body);
                }
            }

            return d;
        }
    }
}
