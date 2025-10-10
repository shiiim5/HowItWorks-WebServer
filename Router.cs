using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webServer
{
        public delegate HttpResponse RouterHandler(HttpRequest request);
    
    public class Router
    {
        private readonly Dictionary<(string, string), RouterHandler> routes = new();

        public void Register(string method, string path, RouterHandler handler)
        {

            routes[(method.ToUpper(), path)] = handler;

        }

        public HttpResponse HandleRequest(HttpRequest request)
        {
            if (routes.TryGetValue((request.Method.ToUpper(), request.Path), out var handler))
            {
                return handler(request);
            }
            else
            {
                return new HttpResponse { Body = "HTTP/1.1 404 Not Found\r\nContent-Type: text/plain\r\n\r\n404 Not Found" };
            }
        }
    }
}