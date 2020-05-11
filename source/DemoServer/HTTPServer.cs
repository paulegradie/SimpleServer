using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace DemoServer
{
    class HTTPServer
    {
        public const string ERROR_DIR = "/root/error_pages/";
        public const string WEB_DIR = "/root/web/";
        public const string VERSION = "HTTP/1.1";
        public const string NAME = "DemoServer";

        private bool is_alive;
        private TcpListener listener;

        public HTTPServer(int port)
        {
            listener = new TcpListener(IPAddress.Any, port);
        }

        public void Start()
        {
            Thread serverThread = new Thread(new ThreadStart(Run));
            serverThread.Start();
        }

        private void Run()
        {
            is_alive = true;
            listener.Start();
            Console.WriteLine("Got to the run command");
            while (is_alive)
            {
                Console.WriteLine("Waiting for a request connection...");

                TcpClient client = listener.AcceptTcpClient();

                HandleClient(client);
                client.Close();
            }

            is_alive = false;
            listener.Stop();
        }

        private void HandleClient(TcpClient client)
        {
  
            using var reader = new StreamReader(client.GetStream());
            string msg = "";
            while (reader.Peek() != -1)
            {
                msg += reader.ReadLine() + "\r\n";
            }
            var splitRequest = msg.Split(new []{Environment.NewLine}, StringSplitOptions.None);
                
            var request = ProcessRequest(splitRequest);
            var response = Response.GenerateResponse(request);
            SendResponse(client.GetStream(), response);
        }

        private static Request ProcessRequest(string[] request)
        {
            if (request.Length == 0)
                return null;

            var meta = RequestParser.ParseRequest(request);
            return new Request(meta.RequestType, meta.URL, meta.HostName);
        }

        private void SendResponse(NetworkStream stream, Response response)
        {
            using var writer = new StreamWriter(stream);
            var header =
                string.Format(
                    format:
                    "{0} {1}\r\nServer: {2}\r\nContent-Type: {3}\r\nAccept-Ranges: bytes\r\nContent-Length: {4}\r\n",
                    HTTPServer.VERSION, response.Status, HTTPServer.NAME, response.Mime, response.Data.Length);

            // this is the header
            writer.WriteLine(header);
            writer.Flush();

            // body
            stream.Write(response.Data, 0, response.Data.Length);
        }
    }
}