using System;
using System.Xml.Serialization;

namespace FLOW.NET.Random
{
    [XmlType("StreamRVGenerator")]
    [XmlInclude(typeof(ExponentialRVGenerator))]
    [XmlInclude(typeof(FixedRVGenerator))]
    [XmlInclude(typeof(LogNormalRVGenerator))]
    [XmlInclude(typeof(NormalRVGenerator))]
    [XmlInclude(typeof(TriangularRVGenerator))]
    [XmlInclude(typeof(UniformRVGenerator))]
    [XmlInclude(typeof(WeibullRVGenerator))]
    public abstract class StreamRVGenerator : RVGenerator
    {
        private RngStream stream;

        public StreamRVGenerator()
        {
            this.stream = new RngStream();
        }

        [XmlIgnore()]
        public RngStream Stream
        {
            get { return this.stream; }
            set { this.stream = value; }
        }

        public override RVGeneratorState GetRVGeneratorState()
        {
            StreamRVGeneratorState state = new StreamRVGeneratorState();
            state.Stream.GetState(this.stream);
            return state;
        }
    }

    [XmlType("StreamRVGeneratorState")]
    public class StreamRVGeneratorState : RVGeneratorState
    {
        private RngStreamState stream;

        public StreamRVGeneratorState()
        {
            stream = new RngStreamState();
        }

        [XmlElement("Stream")]
        public RngStreamState Stream
        {
            get { return this.stream; }
            set { this.stream = value; }
        }

        public override void SetState(RVGenerator generatorIn)
        {
            if (generatorIn.GetType().BaseType.Name == "StreamRVGenerator")
            {
                StreamRVGenerator generator = (StreamRVGenerator)generatorIn;
                this.stream.SetState(generator.Stream);
            }
        }
    }
}