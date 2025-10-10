using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webServer
{
    public class HttpResponse
    {
        public int StatusCode { get; set; } = 200;
        public string StatusMessage { get; set; } = "OK";
        public string ContentType { get; set; }= "text/html";
        public string Body { get; set; }= "<html><body><h1>Hello from my web server!</h1></body></html>";

        public byte[] ToBytes()
        {
            string response = $"HTTP/1.1 {StatusCode} {StatusMessage}\r\n" +
    $"Content-Type: {ContentType}\r\n" +
    $"Content-Length: {Encoding.UTF8.GetByteCount(Body)}\r\n" +
    "\r\n" +
    Body;

            return Encoding.UTF8.GetBytes(response);
  }
    }
}