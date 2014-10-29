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
            //App app = new App();
            //app.Download("monstercat", "favorites", @"C:\Test"); 
            MainAppBehavior(args); 
        }

        static void MainAppBehavior(string[] args)
        {
            String HelpContent = "Welcome to Soundcloud Sync! \nPlease use the following syntax: \nsoundcloud-sync.exe username type (favorites or tracks) path (optional)";
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
                String path = null;
                /* If there is no third argument, set path to null so we don't throw a nullsetexception */
                if (2 < args.Length)
                {
                    path = args[2];
                }

                /* Download the requested content to the requested path! */
                app.Download(username, type, @path);
                //app.Download("digitalsteez", "favorites", @"C:\Test");
            }
        }

    }
}
