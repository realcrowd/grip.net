using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealCrowd.PublishControl
{
    public abstract class Format
    {
        public abstract string Name { get; }

        public abstract object ToObject();
    }
}
