using System;
using System.Xml.Serialization;

namespace FLOW.NET.Random
{
    public enum RVGeneratorType
    {
        BusyTime,
        CalendarTime
    }

    [XmlType("RVGenerator")]
    [XmlInclude(typeof(FileRVGenerator))]
    [XmlInclude(typeof(FixedRVGenerator))]
    [XmlInclude(typeof(StreamRVGenerator))]
    public abstract class RVGenerator : ICloneable
    {
        public RVGenerator()
        {
        }

        public abstract object Clone();

        public abstract double ExpectedValue();

        public abstract double GenerateValue();

        public abstract RVGeneratorState GetRVGeneratorState();
    }

    [XmlType("RVGeneratorState")]
    [XmlInclude(typeof(FileRVGeneratorState))]
    [XmlInclude(typeof(FixedRVGeneratorState))]
    [XmlInclude(typeof(StreamRVGeneratorState))]
    public abstract class RVGeneratorState
    {
        public RVGeneratorState()
        {
        }

        public abstract void SetState(RVGenerator generatorIn);
    }
}