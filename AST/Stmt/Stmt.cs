﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin
{
    internal abstract class Stmt<T>
    {
        internal abstract T Accept(IVisitor<T> visitor);
    }
}
