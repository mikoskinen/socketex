// The code is based on the Crystalbyte Networking for Silverlight. http://cbnet.codeplex.com/
// Modified to work with Windows Phone by Mikael Koskinen. http://mikaelkoskinen.net

using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace SocketEx
{
    /// <summary>
    /// The TcpClient class provides simple methods for connecting, sending, and receiving stream data over a network in synchronous blocking mode.
    /// </summary>
    public class TcpClient : IDisposable
    {
        private readonly AutoResetEvent autoResetEvent;
        private readonly NetworkStream networkStream;
        private EndPoint endpoint;
        private bool responsePending;

        public TcpClient()
            : this(AddressFamily.InterNetwork)
        {
        }


        public TcpClient(IPEndPoint endpoint)
            : this(AddressFamily.InterNetwork)
        {
            Connect(endpoint);
        }

        public TcpClient(string host, int port)
            : this(AddressFamily.InterNetwork)
        {
            var myEndpoint = new DnsEndPoint(host, port);
            InnerConnect(myEndpoint);
        }

        public TcpClient(AddressFamily addressFamily)
        {
            autoResetEvent = new AutoResetEvent(false);

            Client = new Socket(addressFamily, SocketType.Stream, ProtocolType.Tcp);
            networkStream = new NetworkStream(Client);
        }

        public int Available
        {
            get { throw new NotSupportedException(); }
        }

        public Socket Client { get; set; }

        public bool Connected
        {
            get { return Client != null && Client.Connected; }
        }

        public bool Active
        {
            get { return Connected; }
        }

        public bool ExclusiveAddressUse
        {
            get { return false; }
        }

        public bool NoDelay
        {
            get { return true; }
            set { throw new NotImplementedException(); }
        }

        #region IDisposable Members

        public void Dispose()
        {
            var stream = GetStream();
            stream.Dispose();

            try
            {
                Client.Shutdown(SocketShutdown.Both);
                Client.Close();
            }
            catch (ObjectDisposedException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            catch (SocketException ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        #endregion

        public IAsyncResult BeginConnect(IPAddress address, int port, AsyncCallback requestCallback, object userToken)
        {
            endpoint = new IPEndPoint(address, port);
            return BeginConnect(requestCallback, userToken);
        }

        public IAsyncResult BeginConnect(IPAddress[] addresses, int port, AsyncCallback requestCallback,
                                         object userToken)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginConnect(string host, int port, AsyncCallback requestCallback, object userToken)
        {
            endpoint = new DnsEndPoint(host, port);
            return BeginConnect(requestCallback, userToken);
        }

        private IAsyncResult BeginConnect(AsyncCallback requestCallback, object userToken)
        {
            var stateObject = new StateObject
                                  {
                                      AsyncState = userToken,
                                      Callback = requestCallback,
                                      IsCompleted = false
                                  };

            ConnectAsync(stateObject);
            return stateObject;
        }

        private void OnConnectedAsync(object sender, SocketAsyncEventArgs e)
        {
            Continue();

            var stateObject = e.UserToken as StateObject;
            if (stateObject == null) return;

            stateObject.IsCompleted = true;
            stateObject.Callback(stateObject);
        }

        private void OnConnected(object sender, SocketAsyncEventArgs e)
        {
            Continue();
        }

        private void ConnectAsync(StateObject stateObject)
        {
            var e = new SocketAsyncEventArgs { UserToken = stateObject, RemoteEndPoint = endpoint };
            e.Completed += OnConnectedAsync;
            Client.ConnectAsync(e);
        }

        public void Connect(IPEndPoint myEndpoint)
        {
            InnerConnect(myEndpoint);
        }

        public void Connect(IPAddress address, int port)
        {
            var myEndpoint = new IPEndPoint(address, port);
            InnerConnect(myEndpoint);
        }

        public void Connect(IPAddress[] address, int port)
        {
            throw new NotImplementedException();
        }

        public void Connect(string host, int port)
        {
            var myEndPoint = new DnsEndPoint(host, port);
            InnerConnect(myEndPoint);
        }

        protected void InnerConnect(EndPoint myEndpoint)
        {
            this.endpoint = myEndpoint;

            var e = new SocketAsyncEventArgs { RemoteEndPoint = this.endpoint };
            e.Completed += OnConnected;

            try
            {
                Client.ConnectAsync(e);
                WaitOne();

                HandleConnectionReady();
            }
            catch (SocketException)
            {
                Continue();
            }
        }

        protected virtual void HandleConnectionReady()
        {
                
        }

        public void EndConnect(IAsyncResult asyncResult)
        {
            if (!responsePending)
            {
                WaitOne();
            }
        }

        private void Continue()
        {
            responsePending = false;
            autoResetEvent.Set();
        }

        private void WaitOne()
        {
            autoResetEvent.WaitOne();
        }

        public virtual Stream GetStream()
        {
            return networkStream;
        }
    }
}