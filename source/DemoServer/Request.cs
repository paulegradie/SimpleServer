namespace DemoServer
{
    public class Request
    {
        public string Type { get; }
        public string URL { get; }
        public string Host { get; }

        public Request(string type, string url, string host)
        {
            Type = type;
            URL = url;
            Host = host;
        }
    }
}