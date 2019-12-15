using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

namespace FLOW.NET.IO
{
    public abstract class NetworkManager
    {
        private static SortedList<int, TcpListener> listeners = new SortedList<int, TcpListener>();

        public static void WriteToTCPIPPort(Stream streamIn, string addressIn, int portIn)
        {
            TcpClient client = new TcpClient();
            client.Connect(addressIn, portIn);

            NetworkStream networkStream = client.GetStream();
            StreamReader reader = new StreamReader(streamIn);
            StreamWriter writer = new StreamWriter(networkStream);
            writer.Write(reader.ReadToEnd());
            reader.Close();
            writer.Close();
            client.Close();
        }

        public static void CreateTCPListener(int portIn)
        {
            if (listeners.ContainsKey(portIn) == false)
            {
                TcpListener listener = new TcpListener(Dns.GetHostEntry(Dns.GetHostName()).AddressList[0], portIn);
                listener.Start();
                listeners.Add(portIn, listener);
            }
        }

        public static void DestroyTCPListener(int portIn)
        {
            if (listeners.ContainsKey(portIn) == true)
            {
                TcpListener listener = listeners[portIn];
                listener.Stop();
                listeners.Remove(portIn);
            }
        }

        public static TcpListener GetTCPListener(int portIn)
        {
            return listeners[portIn];
        }

        public static string ReadTCPListener(int portIn)
        {
            if (listeners.ContainsKey(portIn) == true)
            {
                TcpListener listener = listeners[portIn];
                while (listener.Pending() == false) ;

                TcpClient client = listener.AcceptTcpClient();
                StreamReader reader = new StreamReader(client.GetStream());
                string result = reader.ReadToEnd();
                client.Close();
                return result;
            }
            else
            {
                return null;
            }
        }
    }
}