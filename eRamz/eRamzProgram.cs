using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eRamz
{
    class eRamzProgram
    {
        static void Main(string[] args)
        {
            try
            {
                if(args.Length == 3)
                {
                    string operation = args[0];
                    string key = args[1];
                    string path = args[2];

                    // Start eRamz
                    eRamzCore.ERamz(operation, key, path);
                }
                else
                {
                    eRamzCore.PrintERamz();
                }
            }
            catch(Exception)
            {
                // Print eRamz
                eRamzCore.PrintERamz();
            }
        }
    }
}
