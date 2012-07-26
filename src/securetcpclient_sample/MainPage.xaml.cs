using System.IO;
using System.Text;
using System.Windows;
using Org.BouncyCastle.Crypto.Tls;
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
            this.Address.Text = string.Format("{0}:{1}", serverAddress, serverPort);
        }

        private void UseSecureClientClick(object sender, RoutedEventArgs e)
        {
            var connection = CreateConnection();
            var stream = connection.GetStream();

            var reader = new StreamReader(stream);
            var writer = new StreamWriter(stream);
            var request = "GET / HTTP/1.1\r\nHost: " + serverAddress + "\r\nConnection: Close\r\n\r\n";

            writer.WriteLine(request);
            writer.Flush();

            var fullMessage = new StringBuilder();

            string message;
            while ((message = reader.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(message))
                    break;

                fullMessage.AppendLine(message);
            }

            ContentSSL.Text = fullMessage.ToString();
        }

        private SecureTcpClient CreateConnection()
        {
            var connection = new SecureTcpClient(serverAddress, serverPort);

            return connection;
        }
    }
}