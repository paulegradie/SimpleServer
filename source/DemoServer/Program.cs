﻿namespace DemoServer
{

    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("Starting a server...");
            HTTPServer server = new HTTPServer(8085);
            server.Start();
        }
    }
}
