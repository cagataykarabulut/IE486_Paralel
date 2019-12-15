using System;
using System.IO;
using System.Net.Sockets;
using System.Xml.Serialization;

namespace FLOW.NET.IO
{
    [XmlType("NetworkOutput")]
    public class NetworkOutput : Output
    {
        private string host;

        private int port;

        public NetworkOutput()
        {
        }

        public NetworkOutput(string hostIn, int portIn)
        {
            this.host = hostIn;
            this.port = portIn;
        }

        [XmlElement("Host")]
        public string Host
        {
            get { return this.host; }
            set { this.host = value; }
        }

        [XmlElement("Port")]
        public int Port
        {
            get { return this.port; }
            set { this.port = value; }
        }

        public override object Clone()
        {
            return new NetworkOutput(this.host, this.port);
        }

        public override Stream GetStream()
        {
            TcpClient client = new TcpClient();
            client.Connect(this.host, this.port);
            return client.GetStream();
        }

        public override string ToString()
        {
            return String.Format("NetworkOutput(host:{0}, port:{1})", this.host, this.port);
        }
    }
}
