using System;
using System.IO;
using System.Threading;
using System.Xml.Serialization;

namespace FLOW.NET.IO
{
    [XmlType("FileInput")]
    public class FileInput : Input
    {
        private bool delete;

        private string path;

        private int poll;

        public FileInput()
        {
        }

        public FileInput(bool deleteIn, string pathIn, int pollIn)
        {
            this.delete = deleteIn;
            this.path = pathIn;
            this.poll = pollIn;
        }

        [XmlElement("Delete")]
        public bool Delete
        {
            get { return this.delete; }
            set { this.delete = value; }
        }

        [XmlElement("Path")]
        public string Path
        {
            get { return this.path; }
            set { this.path = value; }
        }

        [XmlElement("Poll")]
        public int Poll
        {
            get { return this.poll; }
            set { this.poll = value; }
        }

        public override object Clone()
        {
            return new FileInput(this.delete, this.path, this.poll);
        }

        public override Stream GetStream()
        {
            while (File.Exists(this.path) == false)
            {
                Thread.Sleep(this.poll);
            }

            try
            {
                return new FileStream(this.path, FileMode.Open);
            }
            catch
            {
                Thread.Sleep(this.poll);
                return this.GetStream();
            }
        }

        public override void Initialize()
        {
        }

        public override void PostOperation()
        {
            if (this.delete == true)
            {
                File.Delete(this.path);
            }
        }

        public override string ToString()
        {
            return String.Format("FileInput(path:{0}, delete:{1}, poll:{2})", this.path, this.delete, this.poll);
        }
    }
}
