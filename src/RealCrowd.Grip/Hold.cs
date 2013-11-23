// Copyright (c) RealCrowd, Inc. All rights reserved. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealCrowd.Grip
{
    public enum HoldMode
    {
        Response,
        Stream
    }

    public class Hold
    {
        public HoldMode Mode { get; set; }

        public List<Channel> Channels { get; set; }

        public object ToObject()
        {
            var d = new Dictionary<string, object>();

            if (Mode != null && Mode == HoldMode.Stream)
                d["mode"] = "stream";
            else
                d["mode"] = "response";

            var channels = new List<Dictionary<string, object>>();
            foreach (var c in Channels)
            {
                var cObj = new Dictionary<string, object>();
                cObj["name"] = c.Name;
                if (c.PreviousId != null)
                    cObj["prev-id"] = c.PreviousId;
                channels.Add(cObj);
            }
            if (channels.Count() > 0)
                d["channels"] = channels;

            return d;
        }
    }
}
