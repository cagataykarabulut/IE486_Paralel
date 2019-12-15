using System;
using System.Xml.Serialization;

namespace FLOW.NET.Random
{
    [XmlType("FixedRVGenerator")]
    public class FixedRVGenerator : RVGenerator
    {
        private double value;

        public FixedRVGenerator(double valueIn)
        {
            this.value = valueIn;
        }

        public FixedRVGenerator()
        {
        }

        [XmlElement("Value")]
        public double Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        public override object Clone()
        {
            return new FixedRVGenerator(this.value);
        }

        public override double ExpectedValue()
        {
            return this.value;
        }

        public override double GenerateValue()
        {
            return this.value;
        }

        public override RVGeneratorState GetRVGeneratorState()
        {
            FixedRVGeneratorState state = new FixedRVGeneratorState();
            return state;
        }

        public override string ToString()
        {
            return String.Format("Fixed [{0}]", this.value);
        }
    }

    [XmlType("FixedRVGeneratorState")]
    public class FixedRVGeneratorState : RVGeneratorState
    {
        
        public FixedRVGeneratorState()
        {
        }

        public override void SetState(RVGenerator generatorIn)
        {
        }
    }
}