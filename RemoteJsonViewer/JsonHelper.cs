using System;
using System.IO;
using System.Net.Sockets;

namespace JsonTreeView
{
    public class JsonHelper
    {
        private static readonly TcpClient TcpClient = new TcpClient();

        public static void Log(string json)
        {
            if (!TcpClient.Connected)
            {
                var result = TcpClient.ConnectAsync("127.0.0.1", 9128);
                result.Wait();
                if (TcpClient.Connected)
                    Console.WriteLine("jsonViewer: " + (TcpClient.Connected ? "connected" : "cant connect"));
                else
                    return;
            }

            var networkStream = TcpClient.GetStream();
            var clientStreamWriter = new StreamWriter(networkStream);
            clientStreamWriter.Write(json + Environment.NewLine);
            clientStreamWriter.Flush();
        }
    }
}