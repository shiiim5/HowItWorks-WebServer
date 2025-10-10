using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using webServer;


var router = new Router();
router.Register("Get","/",(req)=> new HttpResponse { Body = "<html><body><h1>Hello from my web server - Home Page!</h1></body></html>" });
router.Register("Get","/time",(req)=> new HttpResponse { Body = $"<html><body><h1>{DateTime.Now.ToString()}</h1></body></html>" });
router.Register("Post","/submit",(req)=> new HttpResponse { Body = $"<html><body><h1>{req.Body}</h1></body></html>" });

startServer(router);


static void startServer(Router router)
{

    IPAddress ip = IPAddress.Any;
    int port = 8000;
    IPEndPoint endPoint = new IPEndPoint(ip, port);

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






        //routing and response to client
        var responseText = router.HandleRequest(request);

        clientSocket.Send(responseText.ToBytes());

        clientSocket.Shutdown(SocketShutdown.Both);
        clientSocket.Close();

    }




}
