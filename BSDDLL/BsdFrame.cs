using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BSDDLL
{
    public enum BsdFrameType
    {
        NO_FRAME,
        A_FRAME,
        I_FRAME,
        S_FRAME,
        C_FRAME,
        L_FRAME,
        M_FRAME,
        D_FRAME,
        R_FRAME,
    }

    class BsdFrame
    {
        public BsdFrameType t;
        public byte[] msg;
        public int idbsd = 0;

        /// <summary>
        /// Incoming messages have already been stripped of the frame heading (STX and length) 
        /// and so start with the Frame Type byte and a U16 IDBSD. The full byte array is 
        /// retained in the msg member.
        /// 
        /// </summary>
        /// <param name="data"></param>
        public BsdFrame(byte[] data)
        {
            this.msg = data;
            if(data ==  null || data.Length < 3)
                throw new ArgumentException("bad data");
            switch( data[0] ){
                case (byte)'A': t = BsdFrameType.A_FRAME; break;
                case (byte)'I': t = BsdFrameType.I_FRAME; break;
                case (byte)'S': t = BsdFrameType.S_FRAME; break;
                case (byte)'C': t = BsdFrameType.C_FRAME; break;
                case (byte)'L': t = BsdFrameType.L_FRAME; break;
                case (byte)'M': t = BsdFrameType.M_FRAME; break;
                case (byte)'D': t = BsdFrameType.D_FRAME; break;
                case (byte)'R': t = BsdFrameType.R_FRAME; break;
                default:       t = BsdFrameType.NO_FRAME; break;
            }
            // TODO NO_FRAME type of possible use?
            if(t == BsdFrameType.NO_FRAME)
                throw new ArgumentException("unknown frame type "+data[0]);
            // TODO further decode? or just deal with on case-by-case
            idbsd = Util.u16net(msg[1], msg[2]);

        }
    }

}
