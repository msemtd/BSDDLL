using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Chillitom;

namespace BSDDLL
{
    /// <summary>
    /// Configuration of a BSD unit destined for C-Frame message. 
    /// NB: some of the default values may not be appropriate!
    /// </summary>
    [Serializable]
    public class BsdConfig
    {
        private static readonly ToStringBuilder<BsdConfig> tsb = new ToStringBuilder<BsdConfig>()
            .Include(e => e.revision)
            .Include(e => e.bsdIdentifier)
            .Include(e => e.serialNumber)
            .Include(e => e.port)
            .Include(e => e.server)
            .Include(e => e.nbErrConnectionsBeforeServerDefault)
            .Include(e => e.timeoutGprs)
            .Include(e => e.diagnosticModeActive)
            .Include(e => e.ttsAudioLevel)
            .Include(e => e.powerOffNight)
            .Include(e => e.delayPowerOff)
            .Include(e => e.charEncoding)
            .Include(e => e.battLevelDeepSleep)
            .Include(e => e.battLevelLuminositySwitch)
            .Include(e => e.battLevelTtsShutdown)
            .Include(e => e.highPowerLUT)
            .Include(e => e.lowPowerLUT)
            .Include(e => e.theoricModeChar)
            .Include(e => e.displayScenario)
            .Include(e => e.nbSchedulesDisplayed)
            .Include(e => e.scheduleDisplayDuration)
            .Include(e => e.viaDisplayDuration)
            .Include(e => e.messageDisplayDuration)
            .Include(e => e.messagePresenceFlag)
            .Include(e => e.stopName)
            .Include(e => e.networkName)
            .Include(e => e.footerMessage)
            .Include(e => e.deletedScheduleMessage)
            .Include(e => e.delayedMessage)
            .Include(e => e.unknownedDelayMessage)
            .Include(e => e.keepAliveMessage)
            .Include(e => e.endOfServiceMessage)
            .Include(e => e.busArrivingMessage)
            .Include(e => e.busAtStopMessage)
            .MultiLine(false)               // optionally select formatting
            .Compile();                     // compile to a fast ToString method

        public static void Serialize(string file, BsdConfig c)
        {
            XmlSerializer xs = new XmlSerializer(c.GetType());
            StreamWriter writer = File.CreateText(file);
            xs.Serialize(writer, c);
            writer.Flush();
            writer.Close();
        }

        public static BsdConfig Deserialize(string file)
        {
            XmlSerializer xs = new XmlSerializer(typeof(BsdConfig));
            StreamReader reader = File.OpenText(file);
            BsdConfig c = (BsdConfig)xs.Deserialize(reader);
            reader.Close();
            return c;
        }

        public BsdConfig()
        {
            revision = "01-100101";
            bsdIdentifier = 0;
            serialNumber = "FF FF FF FF FF FF FF FF";
            port = 42042;
            server = "mail.ketech.com";
		    nbErrConnectionsBeforeServerDefault = 100;
		    timeoutGprs = 15;
		    diagnosticModeActive = 1;
		    ttsAudioLevel = 8;
		    powerOffNight = 1;
		    delayPowerOff = 60;
		    charEncoding = 0;
		    battLevelDeepSleep = 1160;
		    battLevelLuminositySwitch = 1230;
		    battLevelTtsShutdown = 1230;
		    highPowerLUT = "{1,5 ; 5,10 ; 15,15 ; 50,20 ; 100,25 ; 600,22 ; 1000,0}";
		    lowPowerLUT = "{1,4 ; 5,5 ; 15,6 ; 50,7 ; 70,5 ; 80,0 ; 300,0}";
		    theoricModeChar = '*';
		    displayScenario = 5;
		    nbSchedulesDisplayed = 2;
		    scheduleDisplayDuration = 5;
		    viaDisplayDuration = 5;
		    messageDisplayDuration = 5;
		    messagePresenceFlag = true;
		    stopName = "stopname";
		    networkName = "networkname";
		    footerMessage = "Current time: %/hhm";
		    deletedScheduleMessage = "delayed schedule message";
		    delayedMessage = "delayed message";
		    unknownedDelayMessage = "unknown delay message";
		    keepAliveMessage = " Northern Rail Route 6 ";
		    endOfServiceMessage = "endofservice message";
		    busArrivingMessage = "arriving";
		    busAtStopMessage = "at-stop";
        }

        public override string ToString()
        {
            return tsb.Stringify(this);
        }

        public string revision { get; set; }
        public int bsdIdentifier { get; set; }
        public string serialNumber { get; set; }
        public int port { get; set; }
        public string server { get; set; }
        public int nbErrConnectionsBeforeServerDefault { get; set; }
        public int timeoutGprs { get; set; }
        public int diagnosticModeActive { get; set; }
        public int ttsAudioLevel { get; set; }
        public int powerOffNight { get; set; }
        public int delayPowerOff { get; set; }
        public int charEncoding { get; set; }
        public int battLevelDeepSleep { get; set; }
        public int battLevelLuminositySwitch { get; set; }
        public int battLevelTtsShutdown { get; set; }
        public string highPowerLUT { get; set; }
        public string lowPowerLUT { get; set; }
        public char theoricModeChar { get; set; }
        public int displayScenario { get; set; }
        public int nbSchedulesDisplayed { get; set; }
        public int scheduleDisplayDuration { get; set; }
        public int viaDisplayDuration { get; set; }
        public int messageDisplayDuration { get; set; }
        public bool messagePresenceFlag { get; set; }
        public string stopName { get; set; }
        public string networkName { get; set; }
        public string footerMessage { get; set; }
        public string deletedScheduleMessage { get; set; }
        public string delayedMessage { get; set; }
        public string unknownedDelayMessage { get; set; }
        public string keepAliveMessage { get; set; }
        public string endOfServiceMessage { get; set; }
        public string busArrivingMessage { get; set; }
        public string busAtStopMessage { get; set; }
    }
}
