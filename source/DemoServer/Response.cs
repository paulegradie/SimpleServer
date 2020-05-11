using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Text;


namespace DemoServer
{
    class Response
    {

        public byte[] Data { get;  }
        public string Status { get;  }
        public string Mime { get; }
        
        private Response(string status, string mime, byte[] data)
        {
            Data = data;
            Mime = mime;
            Status = status;
        }

        public static Response GenerateResponse(Request request)
        {
            if (request == null)
            {
                return MakeErrorResponse("400.html", "400 Bad Request", "text/html");
            }
            if (request.URL == null)
            {
                return MakeErrorResponse("400.html", "400 Bad Request", "text/html");
            }
            else if (request.Type == "GET")
            {
                // this is where my database interface will go.
                string file = Environment.CurrentDirectory + HTTPServer.WEB_DIR + request.URL;
                FileInfo f = new FileInfo(file);

                if (f.Exists && f.Extension.Contains("."))
                {
                    var response = MakeFromFile(f);
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
                return MakeErrorResponse("405.html", "405 Bad Request", "text/html");
            
            return MakeErrorResponse("404.html", "404 Bad Request", "text/html");

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

        private static Response MakeErrorResponse(string filename, string status, string mime)
        {
            var filepath = string.Join("",
                new List<string> {Environment.CurrentDirectory, HTTPServer.ERROR_DIR, filename});
            var file = new FileInfo(filepath);

            using var dataStream = file.OpenRead();
            var fileSize = (int)dataStream.Length;
            
            var reader = new BinaryReader(dataStream);
            var buffer = new byte[fileSize];

            reader.Read(buffer, 0, fileSize);
            return new Response(status, mime, buffer);

        }
        
      
    }
}
