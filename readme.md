# SocketEx - TcpClient and SSL Socket support for Windows Phone #

SocketEx.TcpClient is a MIT-licensed TcpClient for Windows Phone which aims to make working with Windows Phone sockets easy. Compared to the TcpClient in full .NET Framework, SocketEx.TcpClient isn’t 100% compatible and some of the features aren’t implemented at all. The library hasn’t gone through an exhaustive testing so there may be issues.

SocketEx.SecureTcpClient is a TcpClient which provides SSL Socket support to Windows Phone.

Note! The library works in a synchronous blocking mode. This means that if you use the TcpClient directly from the UI-thread, you will block the UI from updating.

The code for TcpClient is based on the “Crystalbyte Networking for Silverlight” project, available from the CodePlex. Almost all of the code is from that neat library, but I adjusted it little to get it working with Windows Phone and fixed out some threading issues.

SecureTcpClient uses Bouncy Castle to provide the SSL support.

## Nuget ##
Both the TcpClient and SecureTcpClient are available from NuGet:

* Install-Package SocketEx
* Install-Package SocketEx.SSL


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
            

##SocketEx.SecureTcpClient - How To open a Secure Connection ##
 
    private SecureTcpClient CreateConnection()
    {
        var connection = new SecureTcpClient(serverAddress, serverPort);

        return connection;
    }

## 3rd part libraries ##
SocketEx.SecureTcpClient uses modified version of Bouncy Castle to create the SSL connections.
## Contact Author ##

Mikael Koskinen - [http://mikaelkoskinen.net]()
