// Copyright (c) RealCrowd, Inc. All rights reserved. See LICENSE in the project root for license information.

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
