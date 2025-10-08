using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webServer
{
    public class HttpRequest
    {
        public string Method { get; set; }
        public string Path { get; set; }
        public string HttpVersion { get; set; }
        public Dictionary<string, string> Headers { get; set; } = new();
        public string Body { get; set; }

    }
}