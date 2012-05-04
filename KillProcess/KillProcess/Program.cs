using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace KillProcess
{
    class Program
    {
        public static void Main(string[] args)
        {
            var processes = new List<Process>();
            foreach (var s in args)
            {
                processes.AddRange(Process.GetProcessesByName(s));
            }
            foreach (var process in processes)
            {
                Console.WriteLine("Killing - {0}", process.ProcessName);
                process.Kill();
            }
            Console.WriteLine("Done!");
            //Console.ReadKey();
        }
    }
}
