// The code is based on the Crystalbyte Networking for Silverlight. http://cbnet.codeplex.com/
// Modified to work with Windows Phone by Mikael Koskinen. http://mikaelkoskinen.net

using System;
using System.Net.Sockets;
using System.Threading;

namespace SocketEx
{
    internal sealed class StateObject : IAsyncResult
    {
        private AutoResetEvent autoResetEvent;
        public int Size { get; set; }
        public AsyncCallback Callback { get; set; }
        public SocketAsyncEventArgs SocketAsyncEventArgs { get; set; }

        #region IAsyncResult Members

        public object AsyncState { get; internal set; }

        public WaitHandle AsyncWaitHandle
        {
            get
            {
                if (autoResetEvent == null)
                {
                    autoResetEvent = new AutoResetEvent(false);
                }

                return autoResetEvent;
            }
        }

        public bool CompletedSynchronously
        {
            get { return false; }
        }

        public bool IsCompleted { get; internal set; }

        #endregion
    }
}