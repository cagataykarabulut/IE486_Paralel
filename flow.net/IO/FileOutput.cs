using System;
using System.IO;
using System.Xml.Serialization;

namespace FLOW.NET.IO
{
    [XmlType("FileOutput")]
    public class FileOutput : Output
    {
        private bool append;

        private string path;

        public FileOutput()
        {
        }

        public FileOutput(bool appendIn, string pathIn)
        {
            this.append = appendIn;
            this.path = pathIn;
        }

        [XmlElement("Append")]
        public bool Append
        {
            get { return this.append; }
            set { this.append = value; }
        }

        [XmlElement("Path")]
        public string Path
        {
            get { return this.path; }
            set { this.path = value; }
        }

        public override object Clone()
        {
            return new FileOutput(this.append, this.path);
        }

        public override Stream GetStream()
        {
            if (this.append == true)
            {
                return new FileStream(this.path, FileMode.Append);
            }
            else
            {
                return new FileStream(this.path, FileMode.Create);
            }
        }

        public override string ToString()
        {
            return String.Format("FileOutput(path:{0}, append:{1})", this.path, this.append);
        }
    }
}
