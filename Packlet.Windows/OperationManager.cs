using System.Collections.Generic;
using Packlet.Core;

namespace Packlet.Windows
{
    public class OperationManager
    {
        public static void RunOperations(List<Operation> operations, string query)
        {
            // Loop through each operation in order:
            foreach (Operation operation in operations)
            {
                switch (operation)
                {
                    case Operation.Install:
                        WindowsPackage.InstallPackage(query);
                        break;
                    case Operation.Remove:
                        WindowsPackage.RemovePackage(query);
                        break;
                    case Operation.Update:
                        WindowsPackage.UpdatePackage(query);
                        break;
                    case Operation.GetVersion:
                        WindowsPackage.GetVersion(query);
                        break;
                }
            }
        }
    }
}