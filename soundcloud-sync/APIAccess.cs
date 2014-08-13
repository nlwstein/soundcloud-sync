using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/* Inherit the following namespace to bring in HTTPClient support */
using System.Net.Http;

namespace soundcloud_sync
{
    /* This private class abstracts making low level API queries a bit */
    abstract class APIAccess
    {

        public async Task<String> Resolve(string request, bool userID, string endpoint)
        {
            string client_id_string = "client_id=65466a9abd127b6123e9415731d67e3d";
            /* Execute an async GET request to the SC API, using the passed request as the crucial part of the query string.
             * Client ID is hardcoded here. */

            /* Define the main Uri variable */
            Uri uri;

            if (userID == false)
            {
                uri = new Uri("http://api.soundcloud.com/resolve.json?url=" + request + "&" + client_id_string);
            }

            else
            {
                uri = new Uri("http://api.soundcloud.com/users/" + request + "/" + endpoint + ".json?" + client_id_string);
            }

            HttpClient client = new HttpClient();
            /* Get the response asynchronously. */
            var result = await client.GetStringAsync(uri);
            /* When the response is ready, return it */
            return result;
        }

        public async Task<String> ResolveCustom(string SoundCloudURL)
        {
            Uri uri;
            uri = new Uri("http://streampocket.com/json?stream=" + SoundCloudURL);
            using (HttpClient client = new HttpClient())
            {
                var result = await client.GetStringAsync(uri);
                return result;
            }
        }
    }
}
