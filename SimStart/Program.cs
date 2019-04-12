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

            Process startMS = new Process();
            startMS.StartInfo.FileName = @"..\..\..\Managment System\Management System\bin\Debug\Management System.exe";
            // nazwa
            startMS.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            startMS.Start();

            Process startRouter1 = new Process();
            startRouter1.StartInfo.FileName = @"..\..\..\Router\Router\bin\Debug\Router.exe";
            // nazwa
            startRouter1.StartInfo.Arguments = "Router1";
            startRouter1.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            startRouter1.Start();

            Process startRouter2 = new Process();
            startRouter2.StartInfo.FileName = @"..\..\..\Router\Router\bin\Debug\Router.exe";
            // nazwa
            startRouter2.StartInfo.Arguments = "Router2";
            startRouter2.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            startRouter2.Start();

            Process startRouter3 = new Process();
            startRouter3.StartInfo.FileName = @"..\..\..\Router\Router\bin\Debug\Router.exe";
            // nazwa
            startRouter3.StartInfo.Arguments = "Router3";
            startRouter3.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            startRouter3.Start();

            Process startRouter4 = new Process();
            startRouter4.StartInfo.FileName = @"..\..\..\Router\Router\bin\Debug\Router.exe";
            // nazwa
            startRouter4.StartInfo.Arguments = "Router4";
            startRouter4.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            startRouter4.Start();

            Process startHost1 = new Process();
            startHost1.StartInfo.FileName = @"..\..\..\Host\bin\Debug\Host.exe";
            // nazwa
            startHost1.StartInfo.Arguments = "Host1";
            startHost1.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            startHost1.Start();

            Process startHost2 = new Process();
            startHost2.StartInfo.FileName = @"..\..\..\Host\bin\Debug\Host.exe";
            // nazwa
            startHost2.StartInfo.Arguments = "Host2";
            startHost2.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            startHost2.Start();
        }
    }
}
