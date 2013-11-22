using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text;

namespace KillProcess
{
    public class Program
    {
        const string Separator = "?";
        public static void Main(string[] args)
        {
            var processes = new List<Process>();
            args.ToList().ForEach(p => 
                {
                    if (p.Contains(Separator))
                    {
                        var processName = p.Substring(0, p.IndexOf(Separator));
                        var potentialProcesses = Process.GetProcessesByName(processName).ToList();
                        potentialProcesses.ForEach(pp =>
                            {
                                if (containsCommandLineFragment(p.Substring(p.IndexOf(Separator) + 1, p.Length - p.IndexOf(Separator) - 1), pp.Id))
                                {
                                    processes.Add(pp);
                                }
                            });
                    }
                    else
                    {
                        processes.AddRange(Process.GetProcessesByName(p));
                    }
                });
            processes.ForEach(process =>
                {
                    Console.WriteLine("Killing - {0}", process.ProcessName);
                    process.Kill();
                });
            Console.WriteLine("Done!");
            //Console.ReadKey();
        }

        static bool containsCommandLineFragment(string commandLineFragment, int processId)
        {
            try
            {
                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT CommandLine FROM Win32_Process WHERE ProcessId = " + processId))
                {
                    foreach (ManagementObject obj in searcher.Get())
                    {
                        return obj["CommandLine"].ToString().ToLower().Contains(commandLineFragment.ToLower());
                    }
                }
            }
            catch (Win32Exception ex)
            {
                if ((uint)ex.ErrorCode != 0x80004005)
                {
                    throw;
                }
            }
            return false;
        }
    }
}
