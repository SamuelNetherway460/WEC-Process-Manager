using System;
using System.Diagnostics;
using System.Linq;
using Terranova.API;
using WECProcessManager.resources;

namespace WECProcessManager
{
    /// <summary>
    /// Main class which implements the main console user interface.
    /// </summary>
    class Program
    {
        private static readonly string TITLE = Constants.APP_NAME + " " + Constants.VERSION_PREFIX + 
                                               Constants.VERSION_NUMBER + Constants.VERSION_POSTFIX;
        private static readonly string MANUAL = "********************************************************\n" + 
                                                "*              " + TITLE + "              *\n" + 
                                                "*                                                      *\n" + 
                                                "* MAN            Launches this manual.                 *\n" + 
                                                "* LIST           Lists all active processes.           *\n" + 
                                                "* KILL -n <path> Kills a process by its <path>.        *\n" + 
                                                "* KILL -p <pid>  Kills a process by its <pid>.         *\n" + 
                                                "* START <path>   Starts a process by its <path>.       *\n" + 
                                                "* QUIT           Quits the application.                *\n" + 
                                                "********************************************************";
        private const string PROCESSES_TABLE_HEADER = "PID\t\tThread Count\tName\n";

        /// <summary>
        /// Prints the manual to the console.
        /// </summary>
        private static void ManualCommand()
        {
            Console.WriteLine(MANUAL);
        }

        /// <summary>
        /// Lists all currently active processes to the console in a table.
        /// Each row contains the full program path, pid and thread count.
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

        /// <summary>
        /// Kills a process by either its pid or full path.
        /// </summary>
        /// <param name="arguments">string array containing all arguments for the command.</param>
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

        /// <summary>
        /// Starts a process by name.
        /// </summary>
        /// <param name="arguments">string array containing all arguments for the command.
        ///                         Typically the full path to the process to start.</param>
        private static void StartCommand(string path)
        {
            Process process = new Process();
            process.StartInfo.FileName = path;
            process.Start();
            Console.WriteLine("Started: " + path);
        }

        /// <summary>
        /// Quits the console application after confirmation.
        /// </summary>
        /// <returns>Boolean indicating whether or not the user has confirmed they still
        ///          wish to quit the application.</returns>
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

        /// <summary>
        /// Simply prints invalid command to the console.
        /// </summary>
        private static void UnknownCommand()
        {
            Console.WriteLine("Invalid command!");
        }

        /// <summary>
        /// Main method.
        /// </summary>
        /// <param name="args">Program arguments. No program arguments are handled.</param>
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
