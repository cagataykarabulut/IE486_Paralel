using System;
using System.IO;
using System.Xml.Serialization;

namespace FLOW.NET.IO
{
    [XmlType("Output")]
    [XmlInclude(typeof(FileOutput))]
    [XmlInclude(typeof(NetworkOutput))]
    public abstract class Output
    {
        public abstract object Clone();

        public abstract Stream GetStream();

        public virtual void Initialize() { }

        public virtual void PreOperation() { }

        public virtual void PostOperation() { }
    }
}
