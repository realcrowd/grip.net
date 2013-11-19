using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealCrowd.PublishControl;

namespace TestApp
{
    public class FppFormat : Format
    {
        public object Value { get; set; }

        public override string Name
        {
            get { return "fpp"; }
        }

        public override object ToObject()
        {
            return Value;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
        }
    }
}
