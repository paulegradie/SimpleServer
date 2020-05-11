using System;
using System.Collections.Generic;


namespace DemoServer
{
    public class RequestParser
    {
        public RequestParser() { }
        public static RequestMeta ParseRequest(string[] lines)
        {
            Dictionary<string, string> KeyValues = new Dictionary<string, string>();
            for (var i = 0; i < lines.Length; i++)
            {
                if (lines[i] != "")
                {
                    AddHeaderData(KeyValues, lines, i);
                }
                else
                {
                    for (var x = i + 1; x < lines.Length; x++)
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
            if (i == 0)
            {
                var main = lines[i].Split(" ");
                KeyValues.Add("request-type", main[0]);
                KeyValues.Add("url", main[1] == "/" ? "index.html" : main[1]);
                KeyValues.Add("protocol", main[2]);
            }
            else
            {
                var key = lines[i].Split(": ")[0];
                var value = lines[i].Split(": ")[1];

                if (key == "host" | key == "Host")
                {
                    var kv = value.Split(":");
                    KeyValues.Add("hostname", kv[0]);
                    KeyValues.Add("port", kv[1]);
                }
                else if (key == "content-type" | key == "Content-Type")
                {
                    KeyValues.Add("content-type", value);
                }
                else
                {
                    KeyValues.Add(key, value);
                }
            }
        }
    }
}