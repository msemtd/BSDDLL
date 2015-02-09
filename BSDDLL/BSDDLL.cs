using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BSDDLL
{
    public class BSDDLL
    {
        private static Logger log = new Logger();
        private static int ccounter = 0;
        private int cc = 0;
        private BsdState state = BsdState.FRESH;
        private int idbsd = 0;
        private string serial = "00 00 00 00 00 00 00 00";
        private BsdFrameType expected = BsdFrameType.NO_FRAME;
        private int tickdown = 0;
        private BsdFrame response = null;
        public int readTimout = 30;
        private StatusResp sr;
        private BsdConfig config;

        /// <summary>
        /// Set services for display
        /// </summary>
        /// <param name="services"></param>
        public void SetServices(XmlDocument services)
        {
            // some services have been received for processing

            // slice and dice XML - how is this done?
            // Form into new internal timetable for H frame

            // we will need to load a table of text for the L frame
            // Additional destination text required?



        }

        /// <summary>
        /// Called every ~1.5 seconds
        /// </summary>
        public void Run()
        {
            try {
                stateMachine(null);
            } catch (Exception e) {
                log.error("state machine ticker failed in state " + state + " with " + e);
                state = BsdState.EXCEPTION;
            }
        }


        private bool stateMachine(BsdFrame f)
        {
            log.debug("state = "+state );
            switch (state) {
            case BsdState.FRESH:
                if (f != null) return enterState(BsdState.PROTOFAULT, "not expecting a message in state "+state);
                cc = (++ccounter);
                prepareIFrameReq();
                return enterState(BsdState.IDENT_WAIT);
            case BsdState.IDENT_WAIT:
                if (f == null && (--tickdown) <= 0) return enterState(BsdState.SLACKING);
                if (f.t != expected) return enterState(BsdState.PROTOFAULT);
                // OK, got ident back from client as expected - is it configured?
                if (f.msg.Length == 11) {
                    // 8-byte serial number...
                    serial = Util.niceByteArray(f.msg, 3, 11);
                    idbsd = Util.u16net(f.msg[1], f.msg[2]);
                    if (idbsd != 0) return enterState(BsdState.PROTOFAULT, "unconfigured BSD has non-zero idbsd");
                } else if (f.msg.Length == 3) {
                    idbsd = Util.u16net(f.msg[1], f.msg[2]);
                } else return enterState(BsdState.PROTOFAULT, "illegal length on I frame response");
                return enterState(BsdState.GET_STATUS);
            case BsdState.GET_STATUS:
                if (f != null) return enterState(BsdState.PROTOFAULT, "not expecting a message in state "+state);
                prepareSFrameReq();
                return enterState(BsdState.STATUS_QUERYWAIT);
            case BsdState.STATUS_QUERYWAIT:
                if (f == null && (--tickdown) <= 0) return enterState(BsdState.SLACKING);
                if (f.t != expected) return enterState(BsdState.PROTOFAULT);
                // OK, got A Frame back from client as expected - must be extended version
                this.sr = new StatusResp(f.msg);
                return enterState(BsdState.CONFIG_TEST);
            case BsdState.CONFIG_TEST:
                if (f != null) return enterState(BsdState.PROTOFAULT, "not expecting a message in state " + state);
                if (sr == null) return enterState(BsdState.PROTOFAULT, "exected status reply to have been set");
                if (serial == null) {
                    log.warn("NB: configured BSD pulls serial from status message");
                    serial = sr.serial;
                }
                // NB: any config must be sent with the current IDBSD for it to
                // be accepted so save for send
                int nb_send_this_id = idbsd;
                if (idbsd == 0) {
                    idbsd = ConfigurationRegistry.lookupIdFromSerial(serial);
                }
                BsdConfig xc = loadConfigForIdbsd(idbsd, serial);
				this.config = xc;
                log.info("Status config revision = " + sr.configRev);
				log.info("Registry config revision = " + xc.revision);


                break; // TODO
            default:
                break;
            }
            log.error("fall-through in state " + state);
            return false;
        }

        public static BsdConfig loadConfigForIdbsd(int idbsd, string serial)
        {
            string xfile = ConfigurationRegistry.idbsdName(idbsd);
            log.info("Load XML config from " + xfile + " ...");
            BsdConfig xc = ConfigurationRegistry.read(xfile);
            log.info("Load XML config from " + xfile + " OK");
            ConfigurationRegistry.validate(serial, xc);
            log.debug("config is " + xc.ToString());
            return xc;
        }

        private void prepareSFrameReq()
        {
            MemoryStream m = new MemoryStream();
            m.WriteByte((byte)'S');
            Util.u16netAdd(m, idbsd);
            Util.nowTimeAdd(m);
            response = new BsdFrame(m.GetBuffer());
        }


        private void prepareIFrameReq()
        {
            // just set the response message to non-zero
            MemoryStream  m = new MemoryStream();
            m.WriteByte((byte)'I');
            m.WriteByte((byte)0xFF);
            m.WriteByte((byte)0xFF);
            Util.u16netAdd(m, readTimout);
            response = new BsdFrame(m.GetBuffer());
        }

        private bool enterState(BsdState s, string p)
        {
            log.error(p);
            return enterState(s);
        }

        private bool enterState(BsdState s)
        {
            log.debug("cc(" + cc + ") bsd("+idbsd+") enters state " + s + " from " + state);
            state = s;
            // the various wait states expect a particular frame type in a limited time
            switch (state)
            {
                case BsdState.IDENT_WAIT:
                    expected = BsdFrameType.I_FRAME;
                    tickdown = 8;
                    break;
                case BsdState.STATUS_QUERYWAIT:
                    expected = BsdFrameType.A_FRAME;
                    tickdown = 8;
                    break;

                default:

                    expected = BsdFrameType.NO_FRAME;
                    tickdown = 0;
                    break;
            }
            return true;
        }


        /// <summary>
        /// Data received from the display (excludes packet header)
        /// </summary>
        /// <param name="data"></param>
        public void ReceivedFromDisplay(byte[] data)
        {
            // Process data
            //......
            //...

            response = null;
            // decode - NB: throws wobbler
            try {
                stateMachine(new BsdFrame(data));
            } catch (Exception e) {
                log.error("state machine for message failed in state " + state + " with " + e);
                state = BsdState.EXCEPTION;
            }

            // Send something back?
            if (e_send != null && (response != null))
            {
                e_send(this, new SendEventArgs(response.msg));
            }

            // Any status to report?
            // This will depend on how we want to translate the internal states
            if (e_status != null)
            {
                e_status(this, new StatusEventArgs("OK"));
            }
        }

        #region send event

        /// <summary>
        /// Display send event arguments
        /// </summary>
        public class SendEventArgs : EventArgs
        {
            /// <summary>
            /// Default constructor
            /// </summary>
            /// <param name="data"></param>
            public SendEventArgs(byte[] data)
            {
                m_data = data;
            }

            /// <summary>
            /// Contains display data
            /// </summary>
            private byte[] m_data = null;

            /// <summary>
            /// Get the data to sent to the display (excludes packet header)
            /// </summary>
            public byte[] Data {get{return m_data;}}
        }

        /// <summary>
        /// The 'Send' delegate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public delegate void Send(object sender, SendEventArgs args);

        /// <summary>
        /// Call this event when you want to send something to the display
        /// </summary>
        public event Send e_send = null;

        #endregion

        #region status event

        /// <summary>
        /// Display status event arguments
        /// </summary>
        public class StatusEventArgs : EventArgs
        {
            /// <summary>
            /// Default constructor
            /// </summary>
            /// <param name="data"></param>
            public StatusEventArgs(string status)
            {
                m_status = status;
            }

            /// <summary>
            /// Contains display status
            /// </summary>
            private string m_status = "NA";

            /// <summary>
            /// Get the status of the display
            /// </summary>
            public string Status { get { return m_status; } }
        }

        /// <summary>
        /// The 'Status' delegate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public delegate void Status(object sender, StatusEventArgs args);

        /// <summary>
        /// Call this event when you want to send the display status to KeAddress
        /// </summary>
        public event Status e_status = null;

        #endregion
    }

    public enum BsdState
    {
        /** newly created BSD connection -  send I message  */
        FRESH,
        /** waiting for ACK response to I message */
        IDENT_WAIT,
        /** time to request full status with S type message */
        GET_STATUS,
        /** waiting for full status ACK response to S message */
        STATUS_QUERYWAIT,
        /** Decision for sending C type message */
        CONFIG_TEST,
        /** Decision for sending C type message */
        SUBLINE_TEST,
        /** Configuration C type message has been sent - waiting for ACK */
        CONFIGURE_WAIT,
        /** a countdown has expired but the socket is still live */
        SLACKING,
        /** Waiting to decide what to do next with this BSD - could have faulted */
        HOLDING_PATTERN,
        /** L type message sent - awaiting ACK */
        SUBLINE_WAIT,
        /** Time to send L type message */
        SEND_H,
        /** H type message sent - awaiting ACK */
        H_ACK_WAIT,
        /**
         * a misunderstanding of the protocol - probably encountered during
         * developemnt or by a confused BSD
         */
        PROTOFAULT,
        /** socket closed cleanly */
        EOF,
        /** something went wrong - check the logs */
        EXCEPTION,
        /** to be tidied up by the server */
        DEAD,
    }

}
