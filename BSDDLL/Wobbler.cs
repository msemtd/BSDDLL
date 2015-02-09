using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BSDDLL
{
    class Wobbler : Exception
    {
        private string p;

        public Wobbler(string p)
        {
            // TODO: Complete member initialization
            this.p = p;
        }
    }
}
