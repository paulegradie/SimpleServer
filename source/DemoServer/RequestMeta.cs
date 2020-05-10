using System.Collections.Generic;


/*
GET / HTTP/1.1
content-type: application/json
User-Agent: PostmanRuntime/7.24.1
Accept: *//*
Postman-Token: 25ab37a7-54f7-4085-a2ba-60b0101be245
Host: localhost:8081
Accept-Encoding: gzip, deflate, br
Connection: keep-alive
Content-Length: 22
// blank line == end of header
"This is a new string"
*/

namespace DemoServer
{
    public readonly struct RequestMeta
    {
        // struct will have a set of predefined key: values we look for, and the rest will be put into a IEnumerable that can be accessed if desired via the this.misc property getter

        public RequestMeta(string url, string hostname, string port, string requestType, string contentType, string protocol, Dictionary<string, string> other)
        {
            RequestType = requestType;
            URL = url;
            HostName = hostname;
            Port = port;
            ContentType = contentType;
            Protocol = protocol;
            OtherMeta = other;
        }
        public Dictionary<string, string> OtherMeta { get; }
        public string HostName { get; }
        public string RequestType { get; }
        public string ContentType { get; }
        public string Protocol { get; }
        public string Port { get; }
        public string URL { get; }


    }





}
