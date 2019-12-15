using System;
using System.IO;
using System.Threading;
using System.Net.Sockets;
using System.Xml.Serialization;

namespace FLOW.NET.IO
{
    [XmlType("NetworkInput")]
    public class NetworkInput : Input
    {
        private int poll;

        private int port;

        public NetworkInput()
        {
        }

        public NetworkInput(int portIn, int pollIn)
        {
            this.port = portIn;
            this.poll = pollIn;
        }

        [XmlElement("Poll")]
        public int Poll
        {
            get { return this.poll; }
            set { this.poll = value; }
        }

        [XmlElement("Port")]
        public int Port
        {
            get { return this.port; }
            set { this.port = value; }
        }

        public override object Clone()
        {
            return new NetworkInput(this.port, this.poll);
        }

        public override Stream GetStream()
        {
            TcpListener listener = NetworkManager.GetTCPListener(this.port);
            while (listener.Pending() == false)
            {
                Thread.Sleep(this.poll);
            }

            TcpClient client = listener.AcceptTcpClient();
            return client.GetStream();
        }

        public override void Initialize()
        {
            NetworkManager.CreateTCPListener(this.port);
        }

        public override string ToString()
        {
            return String.Format("NetworkInput(port:{0}, poll:{1})", this.port, this.poll);
        }
    }
}