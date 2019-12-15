using System;
using System.Xml.Serialization;

namespace FLOW.NET.Random
{
    [XmlType("WeibullRVGenerator")]
    public class WeibullRVGenerator : StreamRVGenerator
    {
        private double alpha;

        private double beta;

        public WeibullRVGenerator(double alphaIn, double betaIn)
        {
            this.alpha = alphaIn;
            this.beta = betaIn;
        }

        public WeibullRVGenerator()
        {
        }

        [XmlElement("Alpha")]
        public double Alpha
        {
            get { return this.alpha; }
            set { this.alpha = value; }
        }

        [XmlElement("Beta")]
        public double Beta
        {
            get { return this.beta; }
            set { this.beta = value; }
        }

        public override object Clone()
        {
            return new WeibullRVGenerator(this.alpha, this.beta);
        }

        public override double ExpectedValue()
        {
            return this.alpha * Math.Pow(-1 * Math.Log(0.5), 1 / this.beta);
        }

        public override double GenerateValue()
        {
            return this.alpha * Math.Pow(-1 * Math.Log(1 - this.Stream.RandU01()), 1 / this.beta);
        }

        public override string ToString()
        {
            return String.Format("Weibull [{0}, {1}]", this.alpha, this.beta);
        }
    }
}