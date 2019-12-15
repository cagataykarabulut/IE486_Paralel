using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLOW.NET.Layout;
using FLOW.NET.Operational;

namespace FLOW.NET.IO
{
    public class TextOutput
    {
        public string path;
        public StreamWriter writer;
        public TextOutput()
        {

        }
        public TextOutput(string PathIn)
        {
            path = PathIn;
            writer = new StreamWriter(this.path);
        }
        public void WriteToText(string data)
        {
            writer.WriteLine(data);
        }
        public void CloseFile()
        {
            writer.Close();
        }
    }
}
