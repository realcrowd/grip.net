using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RealCrowd.PublishControl;

namespace RealCrowd.Grip.Tests
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

    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
        }
    }
}
