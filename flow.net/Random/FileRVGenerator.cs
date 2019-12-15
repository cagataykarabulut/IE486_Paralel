using System;
using System.IO;
using System.Xml.Serialization;

namespace FLOW.NET.Random
{
    [XmlType("FileRVGenerator")]
    public class FileRVGenerator : RVGenerator
    {
        private string filePath;

        private double mean;

        private int position;

        private DoubleList values;

        public FileRVGenerator(string filePathIn)
        {
            this.filePath = filePathIn;
            this.position = -1;
            this.values = new DoubleList();
        }

        public FileRVGenerator()
        {
            this.position = -1;
            this.values = new DoubleList();
        }

        [XmlElement("FilePath")]
        public string FilePath
        {
            get { return this.filePath; }
            set { this.filePath = value; }
        }

        [XmlIgnore()]
        public double Mean
        {
            get { return this.mean; }
            set { this.mean = value; }
        }

        [XmlIgnore()]
        public int Position
        {
            get { return this.position; }
            set { this.position = value; }
        }

        [XmlIgnore()]
        public DoubleList Values
        {
            get { return this.values; }
            set { this.values = value; }
        }

        public override object Clone()
        {
            return new FileRVGenerator(this.filePath);
        }

        public override double ExpectedValue()
        {
            if (this.values.Count == 0)
            {
                this.ReadFromFile();
            }
            return this.mean;
        }

        public override double GenerateValue()
        {
            if (this.values.Count == 0)
            {
                this.ReadFromFile();
            }
            this.position = (this.position + 1) % this.values.Count;
            double value = this.values[0];
            this.values.Add(value);
            this.values.RemoveAt(0);
            return value;
        }

        public override RVGeneratorState GetRVGeneratorState()
        {
            FileRVGeneratorState state = new FileRVGeneratorState();
            state.Position = this.position;
            return state;
        }

        private void ReadFromFile()
        {
            TextReader reader = new StreamReader(this.filePath);
            string[] lines = reader.ReadToEnd().Split('\n');
            reader.Close();
            double sum = 0;
            foreach (string line in lines)
            {
                double value = Double.Parse(line);
                this.values.Add(value);
                sum += value;
            }

            this.mean = sum / this.values.Count;

            for (int i = -1; i < this.position; i++)
            {
                this.values.Add(this.values[0]);
                this.values.RemoveAt(0);
            }
        }

        public override string ToString()
        {
            return String.Format("File [{0}]", this.filePath);
        }
    }


    [XmlType("FileRVGeneratorState")]
    public class FileRVGeneratorState : RVGeneratorState
    {
        private int position;

        [XmlElement("Position")]
        public int Position
        {
            get { return this.position; }
            set { this.position = value; }
        }

        public FileRVGeneratorState()
        {
        }

        public override void SetState(RVGenerator generatorIn)
        {
            if (generatorIn.GetType().Name == "FileRVGenerator")
            {
                FileRVGenerator generator = (FileRVGenerator)generatorIn;
                generator.Position = this.position;
            }
            else
            {
                this.position = -1;
            }
        }
    }
}