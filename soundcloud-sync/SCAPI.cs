using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
/* Inherit the following namespace to bring in Xml support */
using Newtonsoft.Json;

namespace soundcloud_sync
{



    abstract class SCAPI : APIAccess
    {
        public enum DownloadType
        {
            /* Native download link provided by SoundCloud, no work necessary here :) */
            Native = 0,

            /* Downloading using OffLiberty */
            Custom = 1
        }

        protected string GetUserID(string username)
        {
            /* Async get the users page */
            var api = Resolve("https://soundcloud.com/" + username, false, null);

            /* Load the xml from the web service once it has loaded */
            UserResolution user = JsonConvert.DeserializeObject<UserResolution>(api.Result);
            /* Get the first element with name ID and it's inner text, and return that as a string. We now have the user's ID 
             and can therefore look up their favorites */
            return user.id.ToString(); 
            //return this.APIResponse.Elements().First(element => element.Name == "id").Value;

        }

        protected Dictionary<Guid, Tuple<DownloadType, String, String>> GetUserContent(string userID, string endpoint)
        {
            /* Get XML as XML Document */
            var api = Resolve(userID, true, endpoint);
            List<Track> tracks = JsonConvert.DeserializeObject<List<Track>>(api.Result);

            /* Iterate through each entry and store URL and ID to make list */
            Dictionary<Guid, Tuple<DownloadType, String, String>> songs = new Dictionary<Guid, Tuple<DownloadType, String, String>>();

            tracks.ForEach(t =>
            {
                switch(t.downloadable)
                {
                    case true:
                        Console.WriteLine("ID: {0} Link: {1}",t.id.ToString(),t.download_url);
                        songs.Add(Guid.NewGuid(), new Tuple<DownloadType,String,String>(DownloadType.Native,t.id.ToString(), t.download_url.ToString()));
                        break;

                    case false:
                        
                        /* Keep trying to get a response from StreamPocket */
                        bool _response = false;
                        StreamPocketResponse response = new StreamPocketResponse();

                        while (_response == false)
                        {
                            response = JsonConvert.DeserializeObject<StreamPocketResponse>(this.ResolveCustom(t.permalink_url.ToString()).Result);
                            if (response.recorded != null || response.recorded != "")
                            {
                                _response = true;
                            }
                            else { Thread.Sleep(500); }
                        }

                        Console.WriteLine("ID: {0} Link: {1} ", t.id.ToString(), response.recorded);
                        songs.Add(Guid.NewGuid(), new Tuple<DownloadType, String, String>(DownloadType.Custom, t.id.ToString(), response.recorded));
                        break;
                    default:
                        Console.WriteLine("Failed to parse a song.");
                        break;
                    /* Add to our KVP array of songs */
                }
            }); 

            
            /* Return key-value dictionary of songs */
            return songs;
        }

    }
}
