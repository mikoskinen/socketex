using System.IO;
using System.Text;
using System.Threading;
using System.Windows;
using SocketEx;

namespace securetcpclient_sample
{
    public partial class MainPage
    {
        private const string serverAddress = "www.google.fi";
        private const int serverPort = 443;

        public MainPage()
        {
            InitializeComponent();
        }

        private void UseSecureClientClick(object sender, RoutedEventArgs e)
        {
            var connection = CreateConnection();
            var stream = connection.GetStream();

            var reader = new StreamReader(stream);

            ThreadPool.QueueUserWorkItem(x =>
            {
                var fullMessage = new StringBuilder();

                string message;
                while ((message = reader.ReadLine()) != null)
                {
                    fullMessage.AppendLine(message);
                }

                Dispatcher.BeginInvoke(() => Content.Text = fullMessage.ToString());

            });

            using (var writer = new StreamWriter(stream))
            {
                var request = "GET / HTTP/1.1\r\nHost: " + serverAddress + "\r\nConnection: Close\r\n\r\n";

                writer.WriteLine(request);
            }

            Thread.Sleep(1500);
        }

        private SecureTcpClient CreateConnection()
        {
            var connection = new SecureTcpClient(serverAddress, serverPort);

            return connection;
        }
    }
}