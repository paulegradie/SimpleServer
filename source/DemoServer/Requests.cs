using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace DemoServer
{
    class Requests
    {
        public string Type { get; set; }
        public string URL { get; set; }
        public string Host { get; set; }

        private Requests(string type, string url, string host)
        {
            Type = type;
            URL = url;
            Host = host;
        }
    
        public static Requests GetRequests(string request)
        {
            if (string.IsNullOrEmpty(request))
                return null;

            RequestParser parser = new RequestParser();
            RequestMeta meta = parser.ParseRequest(request);

            Console.WriteLine("This did parse successfully!");
            Console.WriteLine("Received a GET request on port: {0}", meta.Port);
            return new Requests(meta.RequestType, meta.URL, meta.HostName);
        }
    }
}
