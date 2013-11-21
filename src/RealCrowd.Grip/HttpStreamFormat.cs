// Copyright (c) RealCrowd, Inc. All rights reserved. See LICENSE in the project root for license information.

using RealCrowd.PublishControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealCrowd.Grip
{
    public class HttpStreamFormat : Format
    {
        public byte[] Content { get; set; }

        public Boolean Close { get; set; }

        public override string Name
        {
            get { return "http-stream"; }
        }

        public override object ToObject()
        {
            var d = new Dictionary<string, object>();
            if (Close)
            {
                d["action"] = "close";
            }
            else
            {
                try
                {
                    d["content"] = Encoding.UTF8.GetString(Content);
                }
                catch (ArgumentException)
                {
                    d["content-bin"] = Convert.ToBase64String(Content);
                }
            }
            return d;
        }
    }
}
