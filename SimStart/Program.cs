using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimStart
{
    class Program
    {
        static void Main(string[] args)
        {
            Process startCloud = new Process();
            startCloud.StartInfo.FileName = @"..\..\..\Connection Cloud\ConnectionCloud\ConnectionCloud\bin\Debug\ConnectionCloud.exe";
            // nazwa
            startCloud.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            startCloud.Start();
        }
    }
}
