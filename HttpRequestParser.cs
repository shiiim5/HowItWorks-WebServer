using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webServer
{
    public class HttpRequestParser
    {
        public static HttpRequest Parse(string requestText)
        {
            var request = new HttpRequest();
            string[] lines = requestText.Split("\r\n");

            var requestLineParts = lines[0].Split(" ");
            if (requestLineParts.Length >= 3)
            {
                request.Method = requestLineParts[0];
                request.Path = requestLineParts[1];
                request.HttpVersion = requestLineParts[2];
            }

            int i = 1;
            for (; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i]))
                {
                    
                    break;
                }

                int separatorIndex = lines[i].IndexOf(":");
                if (separatorIndex > 0)
                {
                    string key = lines[i].Substring(0, separatorIndex).Trim();
                    string value = lines[i].Substring(separatorIndex + 1).Trim();
                    request.Headers[key] = value;
                }
            }

            if (i < lines.Length - 1)
            {
                request.Body = string.Join("\r\n", lines.Skip(i + 1));
            
           }










            return request;
        }
    }
}