using System;
using System.Xml.Serialization;

namespace FLOW.NET.Random
{
    [XmlType("LogNormalRVGenerator")]
    public class LogNormalRVGenerator : StreamRVGenerator
    {
        private double deviation;

        private double mean;

        private double shift;

        private double z1;

        private double z2;

        public LogNormalRVGenerator(double meanIn, double deviationIn, double shiftIn)
        {
            this.mean = meanIn;
            this.deviation = deviationIn;
            this.shift = shiftIn;
            this.z1 = 0;
            this.z2 = 0;
        }

        public LogNormalRVGenerator()
        {
            this.z1 = 0;
            this.z2 = 0;
        }

        [XmlElement("Deviation")]
        public double Deviation
        {
            get { return this.deviation; }
            set { this.deviation = value; }
        }

        [XmlElement("Mean")]
        public double Mean
        {
            get { return this.mean; }
            set { this.mean = value; }
        }

        [XmlElement("Shift")]
        public double Shift
        {
            get { return this.shift; }
            set { this.shift = value; }
        }

        public override object Clone()
        {
            return new NormalRVGenerator(this.mean, this.deviation);
        }

        public override double ExpectedValue()
        {
            double expectedValue;
            expectedValue = this.shift + Math.Exp(this.mean + Math.Pow(this.deviation, 2) / 2);
            return expectedValue;
        }

        public override double GenerateValue()
        {
            if (this.z1 == this.z2)
            {
                do
                {
                    double rand1 = this.Stream.RandU01();
                    double rand2 = this.Stream.RandU01();
                    this.z1 = this.mean + this.deviation * Math.Sqrt(-2 * Math.Log(rand1)) * Math.Cos(2 * Math.PI * rand2);
                    this.z2 = this.mean + this.deviation * Math.Sqrt(-2 * Math.Log(rand1)) * Math.Sin(2 * Math.PI * rand2);
                } while (this.z1 <= 0 || this.z2 <= 0);
                return this.shift + Math.Exp(this.z1);
            }
            else
            {
                this.z1 = this.z2;
                return this.shift + Math.Exp(this.z2);
            }
        }

        public override string ToString()
        {
            return String.Format("LogNormal [{0}, {1},{2}]", this.mean, this.deviation,this.shift);
        }
    }
}