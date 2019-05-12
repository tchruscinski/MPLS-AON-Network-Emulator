using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouterV1
{
        class CommandLineInterface
        {
        private static Parser parser = new Parser();
        private static string ReadRouterConfig(string routerName)
        {
            //Console.WriteLine("przekazana nazwa "+routerName);
            StringBuilder builder = new StringBuilder();
            builder.Append("Router");
            builder.Append(routerName[routerName.Length - 1]);
            routerName = routerName.Remove(routerName.Length - 1);
            builder.Append(".xml");
            string configName = builder.ToString();
            //Console.WriteLine("nazwa szukana "+routerName+" i nazwa pliku: "+configName);
            string routerConfig = parser.ParseLocalConfig(configName);
            return routerConfig;
        }

        private static void DisplayRouterConfig(string routerName)
        {
            string routerConf = "";
            routerConf = ReadRouterConfig(routerName);
            try
            {
                String[] splittedConfig = routerConf.Split(',');
                for(int i = 0; i < splittedConfig.Length - 1; i = i+2)
                {
                    Console.WriteLine("{0}: {1}", splittedConfig[i], splittedConfig[i + 1]);
                }

               
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("Configuration for router {0} doesn't exist.", routerName);
            }

        }
        private static void ShowHelp()
            {
                Console.WriteLine(" ");
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("---POMOC---");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine(" ");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("dlc");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("Wyswietlenie lokalnej konfiguracji");

                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("quit || q");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("Wylaczenie hosta");

            }

            private static void WrongUsage()
            {
                Console.WriteLine(" ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Incorrect usage of CLI.");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("Check help to get more info (\"help\")");
                Console.WriteLine(" ");
            }

            public void RunCommand(string input, Router router)
            {
                String[] comm;
                if (input == "help" || input == "h" || input == "?")
                {
                    ShowHelp();

                }
                else
                {
                    comm = input.Split(' ');


                    if (comm[0] == "dlc")
                    {
                            String localConf = " ";

                            DisplayRouterConfig(router.GetName());   
                            //Console.WriteLine("{0}", localConf);
                    }

                    else if (comm[0] == "quit" || comm[0] == "exit" || comm[0] == "q")
                    {
                        Environment.Exit(0);
                    }
                    else
                    {
                        Console.WriteLine("Invalid command. Please use \"help\" to display help");
                    }
                }
            }


        }
    }
