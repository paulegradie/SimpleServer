using System;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;


namespace DemoServer
{
    class Response
    {

        private byte[] data = null;
        private string status;
        private string mime;

        private Response(string status, string mime, byte[] data)
        {
            this.data = data;
            this.mime = mime;
            this.status = status;
        }


        public static Response From(Requests request)
        {
            if (request == null)
            {
                return MakeNullRequest();
            }
            if (request.URL == null)
            {
                return MakeNullRequest();
            }
            else if (request.Type == "GET")
            {
                // this is where my database interface will go.
                string file = Environment.CurrentDirectory + HTTPServer.WEB_DIR + request.URL;
                FileInfo f = new FileInfo(file);

                if (f.Exists && f.Extension.Contains("."))
                {
                    Console.WriteLine("Sending a Response!");
                    var response = MakeFromFile(f);
                    Console.WriteLine("========= RESPONSE ==========");
                    Console.WriteLine(response);
                    Console.WriteLine("========= RESPONSE END ==========");

                    return response;
                }
                else
                {
                    DirectoryInfo di = new DirectoryInfo(f + "/");
                    FileInfo[] files = di.GetFiles();
                    foreach (FileInfo ff in files)
                    {
                        string n = ff.Name;
                        if (n.Contains("default.html") || n.Contains("default.htm") || n.Contains("index.htm") ||
                            n.Contains("index.html"))
                            MakeFromFile(ff);
                    }

                }
            }
           
            else             
                return MakeNotAllowedResponse();
            
            return MakePageNotFound();

        }

        private static Response MakeFromFile(FileInfo f)
        {
            var fs = f.OpenRead();
            var reader = new BinaryReader(fs);
            var d = new byte[fs.Length];
            reader.Read(d, 0, d.Length);
            fs.Close();
            return new Response("200 OK", "text/html", d);
            
        }

        private static Response MakeNullRequest()
        {
            string file = Environment.CurrentDirectory + HTTPServer.MSG_DIR + "400.html";
            Console.WriteLine(file);
            FileInfo fi = new FileInfo(file);
            FileStream fs = fi.OpenRead();
            BinaryReader reader = new BinaryReader(fs);
            byte[] d = new byte[fs.Length];
            reader.Read(d, 0, d.Length);
            fs.Close();
            return new Response("400 Bad Request", "text/html", d);
        }
        private static Response MakePageNotFound()
        {
            string file = Environment.CurrentDirectory + HTTPServer.MSG_DIR + "404.html";
            Console.WriteLine(file);
            FileInfo fi = new FileInfo(file);
            FileStream fs = fi.OpenRead();
            BinaryReader reader = new BinaryReader(fs);
            byte[] d = new byte[fs.Length];
            reader.Read(d, 0, d.Length);
            fs.Close();
            return new Response("404 Bad Request", "text/html", d);
        }

        private static Response MakeNotAllowedResponse()
        {
            string file = Environment.CurrentDirectory + HTTPServer.MSG_DIR + "405.html";
            Console.WriteLine(file);
            FileInfo fi = new FileInfo(file);
            FileStream fs = fi.OpenRead();
            BinaryReader reader = new BinaryReader(fs);
            byte[] d = new byte[fs.Length];
            reader.Read(d, 0, d.Length);
            fs.Close();
            return new Response("405 Bad Request", "text/html", d);
        }


        public void Post(NetworkStream stream)
        {
            StreamWriter writer = new StreamWriter(stream);
            writer.Flush();
            var header = string.Format(format: "{0} {1}\r\nServer: {2}\r\nContent-Type: {3}\r\nAccept-Ranges: bytes\r\nContent-Length: {4}\r\n", HTTPServer.VERSION, status, HTTPServer.NAME, mime, data.Length);
            Console.WriteLine(header);

            // this is the header
            writer.WriteLine(header);
            // body
            stream.Write(data, 0, data.Length);
        }
    }
}
