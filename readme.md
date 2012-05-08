# SocketEx.TcpClient - TcpClient for Windows Phone #

SocketEx.TcpClient is a MIT-licensed TcpClient for Windows Phone which aims to make working with Windows Phone sockets easy. Compared to the TcpClient in full .NET Framework, SocketEx.TcpClient isn’t 100% compatible and some of the features aren’t implemented at all. The library hasn’t gone through an exhaustive testing so there may be issues.

Note! The library works in a synchronous blocking mode. This means that if you use the TcpClient directly from the UI-thread, you will block the UI from updating.

The code is based on the “Crystalbyte Networking for Silverlight” project, available from the CodePlex. Almost all of the code is from that neat library, but I adjusted it little to get it working with Windows Phone and fixed out some threading issues.

## SocketEx.TcpClient – How To Open a Connection ##
We can open the connection by passing the server address and server port as parameters to TcpClient.

            var serverAddress = "www.google.fi";
            var serverPort = 80;

            var connection = new TcpClient(serverAddress, serverPort);


##SocketEx.TcpClient – How To Receive a Message##
To read a message we need a StreamReader.

            var connection = CreateConnection();
            var stream = connection.GetStream();

            var reader = new StreamReader(stream);

            string message;
            while ((message = reader.ReadLine()) != null)
            {
                Debug.WriteLine(message);
            }

##SocketEx.TcpClient – How To Send a Message##
To write a message we need a StreamWriter.

            var connection = CreateConnection();
            var stream = connection.GetStream();

            using (var writer = new StreamWriter(stream))
            {
                var request = "GET / HTTP/1.1\r\nHost: " + serverAddress + "\r\nConnection: Close\r\n\r\n";

                writer.WriteLine(request);
            }
            
## Contact Author ##

Mikael Koskinen - [http://mikaelkoskinen.net]()
