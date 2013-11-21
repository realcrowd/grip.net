using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealCrowd.PublishControl
{
    public class PublishControlSet
    {
        private List<PublishControl> publishControls = new List<PublishControl>();

        public void Clear()
        {
            publishControls.Clear();
        }

        public void Add(PublishControl publishControl)
        {
            publishControls.Add(publishControl);
        }

        public void ApplyConfiguration(Configuration config)
        {
            foreach (var entry in config.Entries)
            {
                var publishControl = new PublishControl(entry.ControlUri);
                if (entry.ControlIss != null)
                {
                    var claim = new Dictionary<string, object>();
                    claim["iss"] = entry.ControlIss;
                    publishControl.SetAuthJwt(claim, entry.Key);
                }

                publishControls.Add(publishControl);
            }
        }

        public async Task PublishAsync(string channel, Item item)
        {
            var tasks = new List<Task>();
            foreach (var pub in publishControls)
                tasks.Add(pub.PublishAsync(channel, item));

            await Task.WhenAll(tasks);
        }
    }
}
