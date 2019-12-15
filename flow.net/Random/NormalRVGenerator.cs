using System;
using System.Xml.Serialization;

namespace FLOW.NET.Random
{
    [XmlType("NormalRVGenerator")]
    public class NormalRVGenerator : StreamRVGenerator
    {
        private double deviation;

        private double mean;

        private double z1;

        private double z2;

        public NormalRVGenerator(double meanIn, double deviationIn)
        {
            this.mean = meanIn;
            this.deviation = deviationIn;
            this.z1 = 0;
        }

        public NormalRVGenerator()
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

        public override object Clone()
        {
            return new NormalRVGenerator(this.mean, this.deviation);
        }

        public override double ExpectedValue()
        {
            return this.mean;
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
                return this.z1;
            }
            else
            {
                this.z1 = this.z2;
                return this.z2;
            }
        }

        public override string ToString()
        {
            return String.Format("Normal [{0}, {1}]", this.mean, this.deviation);
        }
    }
}