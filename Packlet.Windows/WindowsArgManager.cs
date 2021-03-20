using System;
using System.Collections.Generic;

using Packlet.Core;

namespace Packlet.Windows
{
    public class WindowsArgManager
    {
        public static string Query;
        public static List<Operation> Operations = new List<Operation>();
        
        public void Parse(string[] args)
        {
            foreach (string arg in args)
            {
                if (arg.StartsWith("--")) 
                {
                    switch(arg.Replace("-", string.Empty)) 
                    {
                        // Install
                        case "install":
                            Operations.Add(Operation.Install);
                            break;
                        
                        // Remove
                        case "remove":
                            Operations.Add(Operation.Remove);
                            break;

                        // Update
                        case "update":
                            Operations.Add(Operation.Update);
                            break;
                        
                        // Get Version
                        case "version":
                            Operations.Add(Operation.GetVersion);
                            break;
                    }
                }
                if (arg.StartsWith("-"))
                {
                    foreach (char argChar in arg)
                    {
                        if (argChar != '-')
                        {
                            switch (argChar)
                            {
                                // Install
                                case 'i':
                                    Operations.Add(Operation.Install);
                                    break;

                                // Remove
                                case 'r':
                                    Operations.Add(Operation.Remove);
                                    break;

                                // Update
                                case 'u':
                                    Operations.Add(Operation.Update);
                                    break;
                                
                                // Get Version
                                case 'v':
                                    Operations.Add(Operation.GetVersion);
                                    break;
                            }
                        }
                    }
                }
                else
                {
                    Query = arg;
                }
            }
            
            OperationManager.RunOperations(Operations, Query);
        }
    }
}