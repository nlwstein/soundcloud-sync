using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace soundcloud_sync
{
    /* This class contains the main app logic */

    class App : SCAPI
    {
        /* Highest level abstractions */
        public bool Download(String username, String type, String path)
        {
            /* Instantiate our classes */
            Downloader downloader = new Downloader();
            /* Establish the collection */
            Dictionary<String, String> collection = new Dictionary<String, String>();
            /* Get User Content based on passed type using GetUserId to determine userid based on username */
            collection = GetUserContent(GetUserID(username), type);
            /* Download using path. Error-correction on path designation slash */
            if (!path[path.Length - 1].Equals(@"\")) { path = path + @"\"; }
            path = path + username + @"\" + type;
            /* Download the collection! */
            downloader.DownloadCollection(collection, path);
            return true;
        }

    }

}
