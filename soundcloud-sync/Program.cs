using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace soundcloud_sync
{

    class SCSync
    {

        /* This is the main method that kicks everything off */ 

        static void Main(string[] args)
        {
            String HelpContent = "Welcome to Soundcloud Sync! \nPlease use the following syntax: \nscsync.exe username type (favorites or tracks) path (optional)";
            /* If the user passes nothing into the command line, display help */
            if (args.Length < 2)
            {
                System.Console.WriteLine(HelpContent);
            }
            else
            {
                /* Instantiate the app and download the expected content */ 
                App app = new App();
                
                /* Capture our command line arguments */ 
                String username = args[0];
                String type = args[1];
                String path = args[2];

                /* Download the requested content to the requested path! */ 
                app.Download(username, type, @path);
            }
            /* This makes the app not quit suddenly when finished! */ 
            Console.Write("Press any key to exit...");
            Console.ReadKey();
        }

    }
}
