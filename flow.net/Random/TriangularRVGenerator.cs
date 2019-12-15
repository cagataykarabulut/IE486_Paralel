using System;
using System.Xml.Serialization;

namespace FLOW.NET.Random
{
    [XmlType("TriangularRVGenerator")]
    public class TriangularRVGenerator : StreamRVGenerator
    {
        private double maximum;

        private double median;

        private double minimum;

        public TriangularRVGenerator(double minimumIn, double medianIn, double maximumIn)
        {
            this.minimum = minimumIn;
            this.median = medianIn;
            this.maximum = maximumIn;
        }

        public TriangularRVGenerator()
        {
        }

        [XmlElement("Maximum")]
        public double Maximum
        {
            get { return this.maximum; }
            set { this.maximum = value; }
        }

        [XmlElement("Median")]
        public double Median
        {
            get { return this.median; }
            set { this.median = value; }
        }

        [XmlElement("Minimum")]
        public double Minimum
        {
            get { return this.minimum; }
            set { this.minimum = value; }
        }

        public override object Clone()
        {
            return new TriangularRVGenerator(this.minimum, this.median, this.maximum);
        }

        public override double ExpectedValue()
        {
            double rand = 0.5;
            if (rand <= (this.median - this.minimum) / (this.maximum - this.minimum))
            {
                return this.minimum + Math.Sqrt(rand * (this.median - this.minimum) * (this.maximum - this.minimum));
            }
            else
            {
                return this.maximum - Math.Sqrt((1 - rand) * (this.maximum - this.median) * (this.maximum - this.minimum));
            }
        }

        public override double GenerateValue()
        {
            double rand = this.Stream.RandU01();
            if (rand <= (this.median - this.minimum) / (this.maximum - this.minimum))
            {
                return this.minimum + Math.Sqrt(rand * (this.median - this.minimum) * (this.maximum - this.minimum));
            }
            else
            {
                return this.maximum - Math.Sqrt((1 - rand) * (this.maximum - this.median) * (this.maximum - this.minimum));
            }
        }

        public override string ToString()
        {
            return String.Format("Triangular [{0}, {1}, {2}]", this.minimum, this.median, this.maximum);
        }
    }
}