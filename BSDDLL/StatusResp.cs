using Chillitom;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BSDDLL
{

    class StatusResp
    {
        private static readonly ToStringBuilder<StatusResp> tsb = new ToStringBuilder<StatusResp>()
            .Include(e => e.errcode)
            .Include(e => e.statusSize)
            .Include(e => e.serial)
            .Include(e => e.firmwareRev)
            .Include(e => e.configRev)
            .Include(e => e.sublineRev)
            .Include(e => e.timetableRev)
            .Include(e => e.mailboxRev)
            .Include(e => e.hwConf)
            .Include(e => e.dtime)
            .Include(e => e.batVoltage)
            .Include(e => e.rtCurrentCons)
            .Include(e => e.rtExtCurrent)
            .Include(e => e.rtBatCurrent)
            .Include(e => e.totEnergyCons)
            .Include(e => e.totEnergyConsExt)
            .Include(e => e.totEnergyBatCharge)
            .Include(e => e.rtTemp)
            .Include(e => e.minTemp)
            .Include(e => e.maxTemp)
            .Include(e => e.activeIdleMins)
            .Include(e => e.lastGsmLevel)
            .Include(e => e.gsmRoaming)
            .Include(e => e.serverCons)
            .Include(e => e.severDiscons)
            .Include(e => e.gsmLosses)
            .Include(e => e.rtAmbLux)
            .Include(e => e.lumStatus)
            .Include(e => e.rtBacklightPow)
            .Include(e => e.rtBacklightLutUsed)
            .Include(e => e.ttsReq)
            .Include(e => e.ttsStatus)
            .Include(e => e.ttsErrors)
            .Include(e => e.rtLcdStatus)
            .Include(e => e.rtRtcStatus)
            .Include(e => e.firmwareUpStatus)
            .Include(e => e.firmwareFlashEw)
            .MultiLine(false)               // optionally select formatting
            .Compile();                     // compile to a fast ToString method

        public override string ToString()
        {
            return tsb.Stringify(this);
        }


        public static int MYSTERYNUM = 83;
        public static readonly int longStatusSize = 78;
        
        public int errcode;
        public int statusSize;
        public string serial;
        public int firmwareRev;    // "Firmware revision", 2, "BSD firmware revision"));
        public string configRev;      // "Configuration revision", 4, "Revision field received on the last C configuration frame"));
        public string sublineRev;     // "Subline table revision", 4, "Revision field received on the last L subline frame"));
        public string timetableRev;   // "Timetable revision", 4, "Revision field received on the last T timetable frame"));
        public string mailboxRev;     // "Mailbox revision", 4, "Revision field received on the last M mailbox frame"));
        public int hwConf;         // "Hardware configuration", 1, "bit 0 : 1 if 2 glasses, 0 if 1 glass (LCD), bit 1 : 1 if TTS mounted, else 0, bit 2 : 1 if solar or public lighting power, 0 if permanent external power supply"));
        public string dtime;       // "Date / Time", 7, "7 x byte  Byte 1 : year, Byte 2 : month, Byte 3 : day, Byte 4 : day of week (0 = sunday ... 6 = saturday), Byte 5 : hour, Byte 6 : minute, Byte 7 : second."));
        public int batVoltage;     // "Battery Voltage", 2, "in multiples of 10mV (0,01V), Typically between 1100 and 1500"));
        public int rtCurrentCons;  // "Real time BSD current consumption", 2, "in mA (0,001A), Typically between 1 and 2000"));
        public int rtExtCurrent;   // "Real  time  external input  current  (solar panel  or  external charger power supply)", 2, "in mA, Typically between 0 and 850. This current may be used as well for battery charging than for motherboard and peripherals powering, depending on the conditions (battery voltage, internal temperature)"));
        public int rtBatCurrent; 	// "Real  time  battery charging current", 2, "in mA, Typically between 0 and 850"));
        public int totEnergyCons;	// "Total  BSD  energy consumption, during last 24h", 2, "in multiples of 10mWh (0,01 Wh), Typically between 0 and 3000 Mean value calculated on 24h and updated each day at 8h00 am."));
        public int totEnergyConsExt; // "Total  energy consumption from the external input, during last 24h", 2, "in multiples of 10mWh (0,01 Wh). Typically between 0 and 10 000. Mean value calculated on 24h and updated each day at 8h00 am. May be used by server to detect external power failures, or make solar efficiency measurement."));
        public int totEnergyBatCharge; // "Total battery charging energy consumption during last 24h", 2, "in multiples of 10mWh (0,01 Wh). Typically between 0 and 7000. Mean value calculated on 24h and updated each day at 8h00 am."));
        public int rtTemp; 		// "Real time temperature", 1, "In degrees C, with an offset of +50 degrees C (for example, value=75 for 25 C)"));
        public int minTemp; 		// "Minimum temperature measured during last 24h", 1, "In degrees C, with an offset of +50 C"));
        public int maxTemp; 		// "Maximum temperature measured during last 24h", 1, "In degrees C, with an offset of +50 C"));
        public int activeIdleMins; // "Number  of  minutes while BSD in active or idle state, during last 24h", 2, "in minutes between 0 and 1440. Are not counted minutes spent in abnormal deep sleep state (when battery level is too low). May be used to detect battery or charging problems."));
        public int lastGsmLevel; 	// "Last GSM reception level known", 1, "Give an estimation of the GSM reception quality. Typically between 0 and 31. Value=99 if unknown. This is a logarithmic scale."));
        public int gsmRoaming; 	// "GSM  roaming connection indication", 1, "0x01 if roaming, else 0x00"));
        public int serverCons; 	// 	"Number  of  server connection  retries during last 24h", 2, "Accumulation on 24h and updated each day at 8h00 am."));
        public int severDiscons; 	// "Number  of  server disconnection  events during last 24h", 2, "Accumulation on 24h and updated each day at 8h00 am."));
        public int gsmLosses; 		// "Number  of  GSM network losses during last 24h", 2, "Accumulation on 24h and updated each day at 8h00 am."));
        public int rtAmbLux; 		// "Real  time  ambient luminosity  (measured by front side sensor)", 4, "In multiples of 0,1 lux. Typically between 0 and 1500000"));
        public int lumStatus; 		// "Luminosity  sensor status", 1, "0x01 if ok, 0x00 if any error"));
        public int rtBacklightPow; // "Real  time  backlight power", 1, "Typically between 0 and 100%"));
        public int rtBacklightLutUsed; // "Real  time  backlight LUT used indication", 1, "0x01 if high power LUT, 0x00 if low power LUT. See details about HP and LP LUTs in C configuration frame."));
        public int ttsReq; 		// "Number  of  remote requests  for  TTS during last 24h", 2, "Accumulation on 24h and updated each day at 8h00 am."));
        public int ttsStatus; 		// "Real time TTS module status", 1, "0x01 if in sleep or active mode, 0x00 if powered off"));
        public int ttsErrors; 		// "Number of TTS errors during last 24h", 2, "Accumulation on 24h and updated each day at 8h00 am."));
        public int rtLcdStatus; 	// "Real time LCD display status", 1, "0x02 if active, 0x01 if powered off, 0x00 if error detected. It is only possible to detect a link error or a Master glass failure, but not a Slave glass failure."));
        public int rtRtcStatus;	// 	"Real time internal RTC status", 1, "0x01 if ok, 0x00 if error"));
        public int firmwareUpStatus;// "Firmware  upgrade status", 1, "Status or error code for the firmware upgrade procedure. See section Frame U : firmware upgrade forinformation about the values."));
        public int firmwareFlashEw; // "Total  accumulated number of FLASH E/W cycles performed for firmware upgrade", 2, "Counts the number of Erase/Write cycles performed on the FLASH memory section dedicated to the firmware upgrade. Never cleared by software, always increasing with upgrades. May be used for reliability and statistics purpose."));

        /// <summary>
        /// Decode an incoming A-Frame status response.
        /// </summary>
        /// <param name="ba"></param>
        public StatusResp(byte[] ba)
        {
            if(ba == null || ba.Length != MYSTERYNUM)
                throw new Wobbler("incorrect data length");
            MemoryStream m = new MemoryStream(ba);
            if (Util.u8netRead(m) != (byte)'A')
                throw new Wobbler("msg is not A frame");
            int idbsd = Util.u16netRead(m); // ignore this
            errcode = Util.u8netRead(m);
            statusSize = Util.u8netRead(m);
            if (statusSize != longStatusSize)
                throw new Wobbler("incorrect status size");
            serial = Util.readSerial(m);
            firmwareRev = Util.u16netRead(m);
            configRev = Util.readRev(m);
            sublineRev = Util.readRev(m);   // "Subline table revision", 4, "Revision field received on the last L subline frame"));
            timetableRev = Util.readRev(m); // "Timetable revision", 4, "Revision field received on the last T timetable frame"));
            mailboxRev = Util.readRev(m);   // "Mailbox revision", 4, "Revision field received on the last M mailbox frame"));
            hwConf = Util.u8netRead(m);     // "Hardware configuration", 1, "bit 0 : 1 if 2 glasses, 0 if 1 glass (LCD), bit 1 : 1 if TTS mounted, else 0, bit 2 : 1 if solar or public lighting power, 0 if permanent external power supply"));
            dtime = Util.readDateTime(m);   // "Date / Time", 7, "7 x byte  Byte 1 : year, Byte 2 : month, Byte 3 : day, Byte 4 : day of week (0 = sunday ... 6 = saturday), Byte 5 : hour, Byte 6 : minute, Byte 7 : second."));
            batVoltage = Util.u16netRead(m);     // "Battery Voltage", 2, "in multiples of 10mV (0,01V), Typically between 1100 and 1500"));
            rtCurrentCons = Util.u16netRead(m);  // "Real time BSD current consumption", 2, "in mA (0,001A), Typically between 1 and 2000"));
            rtExtCurrent = Util.u16netRead(m);   // "Real  time  external input  current  (solar panel  or  external charger power supply)", 2, "in mA, Typically between 0 and 850. This current may be used as well for battery charging than for motherboard and peripherals powering, depending on the conditions (battery voltage, internal temperature)"));
            rtBatCurrent = Util.u16netRead(m); 	// "Real  time  battery charging current", 2, "in mA, Typically between 0 and 850"));
            totEnergyCons = Util.u16netRead(m);	// "Total  BSD  energy consumption, during last 24h", 2, "in multiples of 10mWh (0,01 Wh), Typically between 0 and 3000 Mean value calculated on 24h and updated each day at 8h00 am."));
            totEnergyConsExt = Util.u16netRead(m); // "Total  energy consumption from the external input, during last 24h", 2, "in multiples of 10mWh (0,01 Wh). Typically between 0 and 10 000. Mean value calculated on 24h and updated each day at 8h00 am. May be used by server to detect external power failures, or make solar efficiency measurement."));
            totEnergyBatCharge = Util.u16netRead(m); // "Total battery charging energy consumption during last 24h", 2, "in multiples of 10mWh (0,01 Wh). Typically between 0 and 7000. Mean value calculated on 24h and updated each day at 8h00 am."));
            rtTemp = Util.u8netRead(m); 		// "Real time temperature", 1, "In degrees C, with an offset of +50 degrees C (for example, value=75 for 25 C)"));
            minTemp = Util.u8netRead(m); 		// "Minimum temperature measured during last 24h", 1, "In degrees C, with an offset of +50 C"));
            maxTemp = Util.u8netRead(m); 		// "Maximum temperature measured during last 24h", 1, "In degrees C, with an offset of +50 C"));
            activeIdleMins = Util.u16netRead(m); // "Number  of  minutes while BSD in active or idle state, during last 24h", 2, "in minutes between 0 and 1440. Are not counted minutes spent in abnormal deep sleep state (when battery level is too low). May be used to detect battery or charging problems."));
            lastGsmLevel = Util.u8netRead(m); 	// "Last GSM reception level known", 1, "Give an estimation of the GSM reception quality. Typically between 0 and 31. Value=99 if unknown. This is a logarithmic scale."));
            gsmRoaming = Util.u8netRead(m); 	// "GSM  roaming connection indication", 1, "0x01 if roaming, else 0x00"));
            serverCons = Util.u16netRead(m); 	// 	"Number  of  server connection  retries during last 24h", 2, "Accumulation on 24h and updated each day at 8h00 am."));
            severDiscons = Util.u16netRead(m); 	// "Number  of  server disconnection  events during last 24h", 2, "Accumulation on 24h and updated each day at 8h00 am."));
            gsmLosses = Util.u16netRead(m); 		// "Number  of  GSM network losses during last 24h", 2, "Accumulation on 24h and updated each day at 8h00 am."));
            rtAmbLux = Util.u32netRead(m); 		// "Real  time  ambient luminosity  (measured by front side sensor)", 4, "In multiples of 0,1 lux. Typically between 0 and 1500000"));
            lumStatus = Util.u8netRead(m); 		// "Luminosity  sensor status", 1, "0x01 if ok, 0x00 if any error"));
            rtBacklightPow = Util.u8netRead(m); // "Real  time  backlight power", 1, "Typically between 0 and 100%"));
            rtBacklightLutUsed = Util.u8netRead(m); // "Real  time  backlight LUT used indication", 1, "0x01 if high power LUT, 0x00 if low power LUT. See details about HP and LP LUTs in C configuration frame."));
            ttsReq = Util.u16netRead(m); 		// "Number  of  remote requests  for  TTS during last 24h", 2, "Accumulation on 24h and updated each day at 8h00 am."));
            ttsStatus = Util.u8netRead(m); 		// "Real time TTS module status", 1, "0x01 if in sleep or active mode, 0x00 if powered off"));
            ttsErrors = Util.u16netRead(m); 		// "Number of TTS errors during last 24h", 2, "Accumulation on 24h and updated each day at 8h00 am."));
            rtLcdStatus = Util.u8netRead(m); 	// "Real time LCD display status", 1, "0x02 if active, 0x01 if powered off, 0x00 if error detected. It is only possible to detect a link error or a Master glass failure, but not a Slave glass failure."));
            rtRtcStatus = Util.u8netRead(m);	// 	"Real time internal RTC status", 1, "0x01 if ok, 0x00 if error"));
            firmwareUpStatus = Util.u8netRead(m);// "Firmware  upgrade status", 1, "Status or error code for the firmware upgrade procedure. See section Frame U : firmware upgrade forinformation about the values."));
            firmwareFlashEw = Util.u16netRead(m); // "Total  accumulated number of FLASH E/W cycles performed for firmware upgrade", 2, "Counts the number of Erase/Write cycles performed on the FLASH memory section dedicated to the firmware upgrade. Never cleared by software, always increasing with upgrades. May be used for reliability and statistics purpose."));
        }

    }
}
