// Copyright (c) RealCrowd, Inc. All rights reserved. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealCrowd.Grip
{
    public class Instruct
    {
        public Hold Hold { get; set; }
        public Response Response { get; set; }

        public void CreateResponseHold(string channel, string previousId, string timeoutContentType, string timeoutBody)
        {
            Hold = new Hold()
            {
                Mode = HoldMode.Response,
                Channels = new List<Channel>() { new Channel() { Name = channel, PreviousId = previousId } }
            };

            Response = new Response()
            {
                Headers = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("Content-Type", timeoutContentType)
                },
                Body = Encoding.UTF8.GetBytes(timeoutBody)
            };
        }

        public object ToObject()
        {
            var d = new Dictionary<string, object>();
            if (Hold != null)
                d["hold"] = Hold.ToObject();
            if (Response != null)
                d["response"] = Response.ToObject();
            return d;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(ToObject());
        }

        public byte[] GetBytes()
        {
            return Encoding.UTF8.GetBytes(ToString());
        }
    }
}
