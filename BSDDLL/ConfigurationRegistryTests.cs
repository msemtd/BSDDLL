using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSDDLL
{
    [TestClass]
    public class ConfigurationRegistryTests
    {
        [TestMethod]
        public void TestXmlSerialStorage() 
        {
            BsdConfig tc = new BsdConfig();
            tc.serialNumber = "AA AA AA AA 55 55 55 55";
            tc.bsdIdentifier = 1;

            ConfigurationRegistry.validate(tc.serialNumber, tc);

            BsdConfig.Serialize(ConfigurationRegistry.idbsdName(tc.bsdIdentifier), tc);



        }
 


    }
}
