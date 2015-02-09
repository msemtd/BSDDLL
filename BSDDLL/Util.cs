using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSDDLL
{
    /// <summary>
    /// Some handy utility functions. 
    /// 
    /// MemoryStream is used as a convenient flexible byte array for the xxxAdd and xxxRead methods.
    /// 
    /// </summary>
    class Util
    {
        public static readonly byte[] emptyByteArray = new byte[0];

        public static readonly Encoding bsdEnc = Encoding.GetEncoding("Windows-1252");

        public static int u16net(byte msb, byte lsb)
        {
            int v = (int)lsb & 0xff;
            v |= ((int)msb & 0xff) << 8;
            return v;
        }

        public static int u32net(int b, int c, int d, int e)
        {
            int v = (int)e & 0xff;
            v |= ((int)d & 0xff) << 8;
            v |= ((int)c & 0xff) << 16;
            v |= ((int)b & 0xff) << 24;
            return v;
        }

        public static void u16netAdd(MemoryStream m, int v)
        {
            m.WriteByte((byte)((v & 0xff00) >> 8));
            m.WriteByte((byte)(v & 0xff));
        }

        public static int u16netRead(MemoryStream m)
        {
            int msb = m.ReadByte();
            int lsb = m.ReadByte();
            int v = (msb << 8) | lsb;
            return v;
        }

        public static int u32netRead(MemoryStream m)
        {
            int a = m.ReadByte();
            int b = m.ReadByte();
            int c = m.ReadByte();
            int d = m.ReadByte();
            return u32net(a, b, c, d);
        }

        public static int u8netRead(MemoryStream m)
        {
            return m.ReadByte();
        }

        public static byte getByte(int v, int p)
        {
            v >>= (p * 8);
            return (byte)(v & 0xff);
        }

        /// <summary>
        /// Human-readable string representation of a byte array in hex digits
        /// </summary>
        /// <param name="ba">byte array, null is allowed</param>
        /// <param name="stx">start index inclusive</param>
        /// <param name="enx">end index exclusive</param>
        /// <returns></returns>
        public static string niceByteArray(byte[] ba, int stx, int enx)
        {
            StringBuilder sb = new StringBuilder();
            niceByteArrayAdd(sb, ba, stx, enx);
            return sb.ToString();
        }

        /// <summary>
        /// Human-readable byte array
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="ba">byte array, null is allowed</param>
        /// <param name="stx">start index inclusive</param>
        /// <param name="enx">end index exclusive</param>
        public static void niceByteArrayAdd(StringBuilder sb, byte[] ba, int stx, int enx)
        {
            // subarray silent safety...
            if (ba == null) ba = emptyByteArray;
            if (stx < 0) stx = 0;
            if (stx >= ba.Length) stx = 0;
            if ((enx < 0) || (enx >= ba.Length)) enx = ba.Length - 1;
            //
            for (int i = stx; i < enx; i++) {
                byte b = ba[i];
                sb.Append(b.ToString("X2"));
                sb.Append(' ');
            }
            if (sb.Length > 0)
                sb.Remove(sb.Length - 1, 1);
        }

        public static string niceByte(byte b)
        {
            return b.ToString("X2");
        }

        /**
        *  Return the hex digit represented by this number. NB: number must be between
        *  zero and 15 for this to work.
        */
        public static byte hexDigit(byte val)
        {
            return (byte)((val < 0x0A) ? (val + 0x30) : (val + 0x37));
        }

        public static string readRev(MemoryStream m)
        {
            int ver = m.ReadByte();
            int yy = m.ReadByte();
            int mm = m.ReadByte();
            int dd = m.ReadByte();
            return revStr(ver, yy, mm, dd);
        }

        public static string readDateTime(MemoryStream m)
        {
            // "Date / Time", 7, "7 x byte  
            // Byte 1 : year, 
            // Byte 2 : month, 
            // Byte 3 : day, 
            // Byte 4 : day of week (0 = sunday ... 6 = saturday), 
            // Byte 5 : hour, 
            // Byte 6 : minute, 
            // Byte 7 : second.
            int yy = m.ReadByte();
            int mm = m.ReadByte();
            int dd = m.ReadByte();
            int dow = m.ReadByte();
            int hr = m.ReadByte();
            int min = m.ReadByte();
            int sec = m.ReadByte();
            DateTime dt = new DateTime(2000+yy, mm, dd, hr, min, sec, DateTimeKind.Utc);
            return dt.ToString("u") + " dow="+dow+"(="+dt.DayOfWeek+"?)";
        }


        public static string revStr(int ver, int yy, int mm, int dd)
        {
            return String.Format("{0:D2}-{1:D2}{2:D2}{3:D2}", ver, yy, mm, dd);
        }

        public static string readSerial(MemoryStream m)
        {
            byte[] sn = new byte[8];
            m.Read(sn, 0, 8);
            return Util.niceByteArray(sn, 0, 8);
        }

        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        public static void nowTimeAdd(MemoryStream m)
        {
            DateTime dt = DateTime.Now;
            int yy = dt.Year % 100;
            int mm = dt.Month;
            int dd = dt.Day;
            int dow = (int)dt.DayOfWeek;
            int hr = dt.Hour;
            int min = dt.Minute;
            int sec = dt.Second;

            m.WriteByte((byte)yy);
            m.WriteByte((byte)mm);
            m.WriteByte((byte)dd);
            m.WriteByte((byte)dow);
            m.WriteByte((byte)hr);
            m.WriteByte((byte)min);
            m.WriteByte((byte)sec);
        }
    }

}
