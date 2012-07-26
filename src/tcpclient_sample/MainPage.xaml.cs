using System.IO;
using System.Text;
using System.Windows;
using SocketEx;

namespace tcpclient_sample
{
    public partial class MainPage
    {
        private const string serverAddress = "www.google.fi";
        private const int serverPort = 80;

        public MainPage()
        {
            InitializeComponent();
        }

        private void UseClientClick(object sender, RoutedEventArgs e)
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

            Content.Text = fullMessage.ToString();
        }

        private TcpClient CreateConnection()
        {
            var connection = new TcpClient(serverAddress, serverPort);

            return connection;
        }
    }
}