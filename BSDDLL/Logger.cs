using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSDDLL
{
    class Logger
    {
        public void debug(string p)
        {
            say("DEBUG: " + p);
        }

        public void info(string p)
        {
            say("INFO : " + p);
        }

        public void warn(string p)
        {
            say("WARN : " + p);
        }

        public void error(string p)
        {
            say("ERROR: " + p);
        }

        private void say(string p)
        {
            throw new NotImplementedException();
        }
    }
}
