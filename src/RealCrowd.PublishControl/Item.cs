// Copyright (c) RealCrowd, Inc. All rights reserved. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealCrowd.PublishControl
{
    public class Item
    {
        public IList<Format> Formats { get; set; }
        public string Id { get; set; }
        public string PreviousId { get; set; }

        public Item()
        {
            Formats = new List<Format>();
        }

        public Dictionary<string, object> ToDictionary()
        {
            var d = new Dictionary<string, object>();
            if (Id != null)
                d["id"] = Id;
            if (PreviousId != null)
                d["prev-id"] = PreviousId;
            foreach (var format in Formats)
                d[format.Name] = format.ToObject();
            return d;
        }
    }
}
