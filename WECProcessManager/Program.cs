using System;
using System.Diagnostics;
using System.Linq;
using Terranova.API;

namespace ProcessManager
{
    class Program
    {
        private const string MANUAL = "********************************************************\n" +
                                      "*                PROCESS MANAGER v1.0.0                *\n" +
                                      "*                                                      *\n" +
                                      "* MAN            Launches this manual.                 *\n" +
                                      "* LIST           Lists all active processes.           *\n" +
                                      "* KILL -n <name> Kills a process by its <name>.        *\n" +
                                      "* KILL -p <pid>  Kills a process by its <pid>.         *\n" +
                                      "* START <path>   Starts a process by its <path>.       *\n" +
                                      "* QUIT           Quits the application.                *\n" +
                                      "********************************************************";

        private const string PROCESSES_TABLE_HEADER = "PID\t\tThread Count\tName\n";

        //TODO DOCUMENTATION
        /// <summary>
        /// 
        /// </summary>
        private static void ManualCommand()
        {
            Console.WriteLine(MANUAL);
        }

        //TODO DOCUMENTATION
        /// <summary>
        /// 
        /// </summary>
        private static void ListProcessesCommand()
        {
            Console.WriteLine(PROCESSES_TABLE_HEADER);
            ProcessInfo[] processes = ProcessCE.GetProcesses();
            foreach (ProcessInfo p in processes)
            {
                if (p.Pid.ToString().Length > 7)
                {
                    Console.WriteLine(p.Pid + "\t" + p.ThreadCount + "\t\t" + p.FullPath);
                }
                else
                {
                    Console.WriteLine(p.Pid + "\t\t" + p.ThreadCount + "\t\t" + p.FullPath);
                }
            }
        }

        //TODO DOCUMENTATION
        /// <summary>
        /// 
        /// </summary>
        private static void KillProcessCommand(string[] arguments)
        {
            ProcessInfo[] processes = ProcessCE.GetProcesses();
            if (arguments[0].ToLower().Equals("-n"))
            {
                foreach (ProcessInfo p in processes)
                {
                    if (p.FullPath.Equals(arguments[1]))
                    {
                        p.Kill();
                        Console.WriteLine("Killed: " + p.Pid + " | " + p.FullPath);
                    }
                }
            }
            else if (arguments[0].ToLower().Equals("-p"))
            {
                foreach (ProcessInfo p in processes)
                {
                    if (p.Pid.ToString() == arguments[1])
                    {
                        p.Kill();
                        Console.WriteLine("Killed: " + p.Pid + " | " + p.FullPath);
                    }
                }
            }
            else
            {
                Console.WriteLine("Invalid arguments! Please try again.");
            }
        }

        //TODO DOCUMENTATION
        //TODO TEST
        //TODO IMPLEMENT
        /// <summary>
        /// 
        /// </summary>
        /// <param name="arguments"></param>
        private static void StartCommand(string path)
        {
            Process process = new Process();
            process.StartInfo.FileName = path;
            process.Start();
            Console.WriteLine("Started: " + path);
        }

        //TODO DOCUMENTATION
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static bool QuitCommand()
        {
            string decision = "start";
            bool invalidInput = false;
            Console.Write("Are you sure you want to quit? (Y/N): ");
            decision = Console.ReadLine();
            do
            {
                if (decision.ToLower().Equals("y") || decision.ToLower().Equals("n"))
                {
                    return decision.ToLower().Equals("y");
                }
                else
                {
                    invalidInput = true;
                    Console.Write("Invalid input! Are you sure you want to quit? (Y/N): ");

                }
                decision = Console.ReadLine();
            } while (invalidInput);

            return decision.ToLower().Equals("y");
        }

        //TODO DOCUMENTATION
        //TODO TEST
        /// <summary>
        /// 
        /// </summary>
        private static void UnknownCommand()
        {
            Console.WriteLine("Invalid command!");
        }

        //TODO DOCUMENTATION
        //TODO IMPLEMENT
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            Console.WriteLine(MANUAL);
            while (true)
            {
                Console.Write("> ");
                string entry = Console.ReadLine();
                if (!string.IsNullOrEmpty(entry))
                {
                    string[] splitEntry = entry.Split(' ');
                    string command = splitEntry[0];
                    string[] arguments = splitEntry.Skip(1).ToArray();

                    bool quit = false;

                    switch (command.ToLower())
                    {
                        case "man":
                            ManualCommand();
                            break;
                        case "list":
                            ListProcessesCommand();
                            break;
                        case "kill":
                            KillProcessCommand(arguments);
                            break;
                        case "start":
                            StartCommand(arguments[0]);
                            break;
                        case "quit":
                            quit = QuitCommand();
                            break;
                        default:
                            UnknownCommand();
                            break;
                    }

                    if (quit) break;
                }
            }
        }
    }
}
