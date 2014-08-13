using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Reflection;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace soundcloud_sync
{
    /* This class contains the main app logic */

    class App : SCAPI
    {
        public static Dictionary<Guid, Tuple<DownloadType, String, String>> Collection { get; set; }
        
        /* Highest level abstractions */
        public bool Download(String username, String type, String path)
        {
            /* Instantiate our classes */
            Downloader downloader = new Downloader();
           
            /* Get User Content based on passed type using GetUserId to determine userid based on username */
            Collection = GetUserContent(GetUserID(username), type);
            
            /* Let's concatenate our path. */
            if (path == null) { path = Assembly.GetAssembly(typeof(App)).Location; }
			path = Path.Combine(path,username,type);
            
            /* Download the collection! */
            downloader.DownloadCollection(Collection, path);
            return true;
        }

    }

}
