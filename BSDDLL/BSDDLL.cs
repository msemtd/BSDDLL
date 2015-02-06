using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BSDDLL
{
    public class BSDDLL
    {
        /// <summary>
        /// Set services for display
        /// </summary>
        /// <param name="services"></param>
        public void SetServices(XmlDocument services)
        {
            // some services have been received for processing
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

            // Send something back?
            if (e_send != null)
            {
                byte[] some_data = null;
                e_send(this, new SendEventArgs(some_data));
            }

            // Any status to report?
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
}
