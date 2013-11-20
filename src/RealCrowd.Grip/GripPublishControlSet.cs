using RealCrowd.PublishControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealCrowd.Grip
{
    public class GripPublishControlSet : PublishControlSet
    {
        public async Task PublishAsync(string channel, string id, string prevId, HttpResponseFormat responseFormat = null, HttpStreamFormat streamFormat = null)
        {
            var item = new Item() { Id = id, PrevId = prevId };

            if (responseFormat != null)
                item.Formats.Add(responseFormat);
            if (streamFormat != null)
                item.Formats.Add(streamFormat);

            await PublishAsync(channel, item);
        }
    }
}
