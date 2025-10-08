// See https://aka.ms/new-console-template for more information
using System.Net;
using System.Net.Sockets;
using System.Text;
using webServer;


IPAddress ip = IPAddress.Any;
        int port = 8000;
        IPEndPoint endPoint = new IPEndPoint(ip,port);

        Socket webSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
webSocket.Bind(endPoint);

webSocket.Listen(10);
Console.WriteLine("Listening\n");

while (true)
{
    var clientSocket = webSocket.Accept();
    Console.WriteLine("Client connected\n");

    //recieve data from client
    byte[] buffer = new byte[1024];
    int recievedBytes = clientSocket.Receive(buffer);
    string requestText = Encoding.UTF8.GetString(buffer, 0, recievedBytes);
    Console.WriteLine("Request: \n\n\n" + requestText);

    //parsing request
   var request = HttpRequestParser.Parse(requestText);
   Console.WriteLine("Method: \n\n" + request.Method);
   Console.WriteLine("Path: \n\n" + request.Path);
   Console.WriteLine("HttpVersion: \n\n" + request.HttpVersion);
   Console.WriteLine("Content-Length: \n\n" + request.Headers["Content-Length"]);
   Console.WriteLine("Body: \n\n" + request.Body);



   
   
    //response to client
    string responseText = "HTTP/1.1 200 OK\r\n Content-Type: text/plain\r\n\n<html><body><h1>Hello from my web server!</h1></body></html>";
    byte[] responseBytes = Encoding.UTF8.GetBytes(responseText);
    clientSocket.Send(responseBytes);

    clientSocket.Shutdown(SocketShutdown.Both);
    clientSocket.Close();

}



