using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BSDDLL
{
    class ConfigurationRegistry
    {
        public static readonly string xmlFilePrefix = "bsdconf_";
	    private static readonly string xmlFileRegex = "^" + xmlFilePrefix + "(\\d+)" + "\\.xml";
        public static readonly string defaultSerialNumber = "FF FF FF FF FF FF FF FF";
	    public static readonly int defaultIdbsd = 0;

        public static int lookupIdFromSerial(string serial)
        {
            // TODO the system should hold a mapping from serial number to IDBSD for all known BSD units
            // this could be in a database or simple XML file
            return 666;
        }


        public static string idbsdName(int idbsd)
        {
            return xmlFilePrefix + idbsd + ".xml";
        }

        public static void checkSerialNumbers(string serial, BsdConfig xc)
        {
            throw new NotImplementedException();
        }

        public static void validate(string serial, BsdConfig xc)
        {
            // validate both serial number formats 
            // validate serial against given serial
            // validate revision number format



            throw new NotImplementedException();
        }

        public static BsdConfig read(string xfile)
        {
            return BsdConfig.Deserialize(xfile);
        }
    }
}
