using System;
using System.IO;
using System.Xml.Serialization;

namespace FLOW.NET.IO
{
    [XmlType("Input")]
    [XmlInclude(typeof(FileInput))]
    [XmlInclude(typeof(NetworkInput))]
    public abstract class Input : ICloneable
    {
        public abstract object Clone();

        public abstract Stream GetStream();

        public virtual void Initialize() { }

        public virtual void PreOperation() { }

        public virtual void PostOperation() { }
    }
}
