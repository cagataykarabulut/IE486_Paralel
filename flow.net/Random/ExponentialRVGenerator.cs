using System;
using System.Xml.Serialization;

namespace FLOW.NET.Random
{
    [XmlType("ExponentialRVGenerator")]
    public class ExponentialRVGenerator : StreamRVGenerator
    {
        private double lambda;

        public ExponentialRVGenerator()
        {
        }

        public ExponentialRVGenerator(double lambdaIn)
        {
            this.lambda = lambdaIn;
        }

        [XmlElement("Lambda")]
        public double Lambda
        {
            get { return this.lambda; }
            set { this.lambda = value; }
        }

        public override object Clone()
        {
            return new ExponentialRVGenerator(this.lambda);
        }

        public override double ExpectedValue()
        {
            return 1 / this.lambda;
        }

        public override double GenerateValue()
        {
            return -1 * Math.Log(1 - this.Stream.RandU01()) / this.lambda;
        }

        public override string ToString()
        {
            return String.Format("Exponential [{0}]", this.lambda);
        }
    }
}