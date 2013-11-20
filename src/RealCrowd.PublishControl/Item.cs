using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealCrowd.PublishControl
{
    public class Item
    {
        public List<Format> Formats { get; set; }

        public string Id { get; set; }

        public string PrevId { get; set; }

        public Dictionary<string, object> ToDictionary()
        {
            var d = new Dictionary<string, object>();
            if (Id != null)
                d["id"] = Id;
            if (PrevId != null)
                d["prev-id"] = PrevId;
            foreach (var format in Formats)
                d[format.Name] = format.ToObject();
            return d;
        }
    }
}
