using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Threading.Tasks;

namespace webServer
{
        public delegate HttpResponse RouterHandler(HttpRequest request);
    
    public class Router
    {
        private readonly List<(string Method, string Path, RouterHandler Handler)> routes = new();

        public void Register(string method, string path, RouterHandler handler)
        {

            routes.Add((method.ToUpper(), path, handler));

        }

        public HttpResponse HandleRequest(HttpRequest request)
        {
            foreach (var route in routes)
            {
                if (!string.Equals(request.Method, route.Method, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                var variables = HttpRequestParser.MatchRoutingVariables(route.Path, request.Path);
               
                if (variables != null)
                {
                    request.Variables = variables;

                    return route.Handler(request);

                }


            }
            return new HttpResponse
            {
                StatusCode = 404,
                StatusMessage = "Not Found",
                ContentType = "text/html",
Body = "<html><body><h1>404 - Not Found</h1><body/></html>"

            };
            
        }
   
      
    }
}