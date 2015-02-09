using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;

namespace BSDDLL
{
    [TestClass]
    public class StausRespTests
    {
        [TestMethod]
        public void TestDecodingStatusResponse()
        {
            string msg = "41 00 00 00 4E A8 02 02 24 22 00 10 25 00 08 00 0A 01 01 00 0A 01 01 00 0A 01 01 00 0A 01 01 01 0F 01 1C 04 02 18 1D 05 5E 00 27 00 42 00 1B 00 00 00 00 00 00 4A 47 47 00 00 17 00 00 00 00 00 00 00 00 00 01 AE 01 13 01 00 00 00 00 00 02 01 00 00 00";
            msg = Regex.Replace(msg, @"\s", "");
            byte[] ba = Util.StringToByteArray(msg);
            BsdFrame f = new BsdFrame(ba);
            StatusResp r = new StatusResp(ba);
            Console.WriteLine(""+r.ToString());
               
        }


    }
}
