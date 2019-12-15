using System;
using System.Xml.Serialization;

namespace FLOW.NET.Random
{
    [XmlType("UniformRVGenerator")]
    public class UniformRVGenerator : StreamRVGenerator
    {
        private double maximum;

        private double minimum;

        public UniformRVGenerator(double minimumIn, double maximumIn)
        {
            this.minimum = minimumIn;
            this.maximum = maximumIn;
        }

        public UniformRVGenerator()
        {
        }

        [XmlElement("Maximum")]
        public double Maximum
        {
            get { return this.maximum; }
            set { this.maximum = value; }
        }

        [XmlElement("Minimum")]
        public double Minimum
        {
            get { return this.minimum; }
            set { this.minimum = value; }
        }

        public override object Clone()
        {
            return new UniformRVGenerator(this.minimum, this.maximum);
        }

        public override double ExpectedValue()
        {
            return (this.minimum + this.maximum) / 2;
        }

        public override double GenerateValue()
        {
            return this.minimum + (this.maximum - this.minimum) * this.Stream.RandU01();
        }

        public override string ToString()
        {
            return String.Format("Uniform [{0}, {1}]", this.minimum, this.maximum);
        }
    }
}