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

        public const string MSG_DIR = "/root/msg/";
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
                Console.WriteLine("Waiting for a request connection..." );

                TcpClient client = listener.AcceptTcpClient();

                HandleClient(client);
                client.Close();
            }
            is_alive = false;
            listener.Stop();
        }

        private void HandleClient(TcpClient client)
        {
            StreamReader reader = new StreamReader(client.GetStream());

            string msg = "";
            while (reader.Peek() != -1)
            {
                msg += reader.ReadLine() + "\n";
            }

            Console.WriteLine("------------------------------\n");
            Console.WriteLine("Request: \n" + msg);
            Console.WriteLine("------------------------------");

            Requests req = Requests.GetRequests(msg);
            Response res = Response.From(req);
            res.Post(client.GetStream());

        }
    }
}
