// Copyright (c) RealCrowd, Inc. All rights reserved. See LICENSE in the project root for license information.

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RealCrowd.PublishControl;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;

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
            var reader = new StreamReader(Environment.GetEnvironmentVariable("RC_GRIP_CONFIG"));
            var config = JsonConvert.DeserializeObject<Dictionary<string, object>>(reader.ReadToEnd());
            var pub = new PublishControlSet();
            pub.ApplyConfiguration(new PublishControl.Configuration(config["gripProxiesString"].ToString()));
            pub.PublishAsync("test", new Item() { Formats = new List<Format>() { new FppFormat() { Value = "hello" } } }).Wait();
        }
    }
}
