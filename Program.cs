using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using webServer;


var router = new Router();
router.Register("Get","/",(req)=> new HttpResponse { Body = "<html><body><h1>Hello from my web server - Home Page!</h1></body></html>" });
router.Register("Get","/time",(req)=> new HttpResponse { Body = $"<html><body><h1>{DateTime.Now.ToString()}</h1></body></html>" });
router.Register("Get","/numbers/{number}",GetNumber);
router.Register("Get","/words/{word}",GetWord);
router.Register("Post","/submit",login);
router.Register("Get","/search",Search);


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

        Console.WriteLine("Requested path: \n\n\n" + request.Path);







        //routing and response to client
        var responseText = router.HandleRequest(request);
        Console.WriteLine("======================================================= \n\n\n");



        clientSocket.Send(responseText.ToBytes());
        
        Console.WriteLine($"status: {responseText.StatusCode}, {responseText.StatusMessage} \n\n\n");


        clientSocket.Shutdown(SocketShutdown.Both);
        clientSocket.Close();

    }




}

static HttpResponse login(HttpRequest request)
{


    string credentials = request.Body;
    string userName = credentials.Split('&')[0].Split('=')[1];
    string password = credentials.Split('&')[1].Split('=')[1];

    if (password == "123")
        return new HttpResponse { Body = $"<html><body><h1></h1>Hello {userName}</body></html>" };
    else
        return new HttpResponse
        {
            StatusCode = 401,
            StatusMessage = "Unauthorized",
            ContentType = "text/plain",
            Body = "Unauthorized"
        };

}

static HttpResponse GetNumber(HttpRequest request)
{
    string number = request.Variables["number"];
    return new HttpResponse
    {
        Body = $"<html><body><h1>Number is {number}</h1></body></html>"

    };



}

static HttpResponse GetWord(HttpRequest request)
{
    string word = request.Variables["word"];
    return new HttpResponse
    {
        Body = $"<html><body><h1>Word is {word}</h1></body></html>"

    };



}

static HttpResponse Search(HttpRequest request)
{
    string keyword = request.QueryStrings.ContainsKey("keyword") ? request.QueryStrings["keyword"] : "none";
    string limit = request.QueryStrings.ContainsKey("limit") ? request.QueryStrings["limit"] : "10";

    return new HttpResponse
    {
        Body = $"<html><body><h1>Search Results for '{keyword}' with limit {limit}</h1></body></html>"

    };
}


