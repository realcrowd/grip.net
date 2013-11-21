// Copyright (c) RealCrowd, Inc. All rights reserved. See LICENSE in the project root for license information.

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
        public byte[] Body { get; set; }

        public HttpResponseFormat(string contentType, string body)
        {
            Headers = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("Content-Type", contentType)
            };
            Body = Encoding.UTF8.GetBytes(body);
        }

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
            try
            {
                d["body"] = Encoding.UTF8.GetString(Body);
            }
            catch(ArgumentException)
            {
                d["body-bin"] = Convert.ToBase64String(Body);
            }
            return d;
        }
    }
}
