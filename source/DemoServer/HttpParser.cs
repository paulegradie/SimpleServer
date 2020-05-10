using System;
using System.Collections.Generic;


namespace DemoServer
{
    public class RequestParser
    {
        // the request parser will ultimately return an HttpBody struct which will provide access to the request body via property getters
        public RequestParser() { }

        public RequestMeta ParseRequest(string request)
        {

            var parsedRequest = parseRequest(request);

            return parsedRequest;
        }

        private static RequestMeta parseRequest(string request)
        {

            Dictionary<string, string> KeyValues = new Dictionary<string, string>();

            string[] lines = request.Split("\n");
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i] != "")
                {
                    AddHeaderData(KeyValues, lines, i);
                } else
                {
                    for (int x = i+1; x < lines.Length; x++)
                    {
                        AddBodyData(KeyValues, lines, x);

                    }
                    break;
                }
            };

            KeyValues.TryGetValue("url", out string url);
            KeyValues.TryGetValue("hostname", out string hostname);
            KeyValues.TryGetValue("port", out string port);
            KeyValues.TryGetValue("content-type", out string contentType);
            KeyValues.TryGetValue("request-type", out string requestType);
            KeyValues.TryGetValue("protocol", out string protocol);

            return new RequestMeta(url, hostname, port, requestType, contentType, protocol, KeyValues);

        }

        private static void AddBodyData(Dictionary<string, string> keyValues, string[] lines, int i)
        {
            keyValues.TryGetValue("body", out string body);
            body += lines[i];
            keyValues.Remove("body");
            keyValues.Add("body", body);
        }

        private static void AddHeaderData(Dictionary<string, string> KeyValues, string[] lines, int i)
        {
            string key;
            string value;

            if (i == 0)
            {
                var main = lines[i].Split(" ");
                KeyValues.Add("request-type", main[0]);

                if (main[1] == "/")
                {
                    KeyValues.Add("url", "index.html");
                } else
                {
                    KeyValues.Add("url", main[1]);
                }

                KeyValues.Add("protocol", main[2]);
            }
            else
            {

                key = lines[i].Split(": ")[0];
                value = lines[i].Split(": ")[1];

                if (key == "host" | key == "Host")
                {
                    string[] kv = value.Split(":");
                    KeyValues.Add("hostname", kv[0]);
                    KeyValues.Add("port", kv[1]);

                }
                else if (key == "content-type" | key == "Content-Type")
                {
                    KeyValues.Add("content-type", value);
                } else
                {
                    KeyValues.Add(key, value);
                }
            }
        }
    }
}
