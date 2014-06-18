using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/* Inherit the following namespace to bring in Xml support */
using System.Xml;

namespace soundcloud_sync
{
    abstract class SCAPI : APIAccess
    {
        protected string GetUserID(string username)
        {
            /* Async get the users page */
            var api = Resolve("https://soundcloud.com/" + username, false, null);
            /* Create a new logical XmlDocument */
            XmlDocument xml = new XmlDocument();
            /* Load the xml from the web service once it has loaded */
            xml.LoadXml(api.Result);
            /* Get the first element with name ID and it's inner text, and return that as a string. We now have the user's ID 
             and can therefore look up their favorites */
            return xml.GetElementsByTagName("id")[0].InnerText;

        }

        protected Dictionary<String, String> GetUserContent(string userID, string endpoint)
        {
            /* Get XML as XML Document */
            var api = Resolve(userID, true, endpoint);
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(api.Result);
            /* Iterate through each entry and store URL and ID to make list */
            Dictionary<String, String> songs = new Dictionary<String, String>();

            foreach (XmlElement track in xml.GetElementsByTagName("track"))
            {

                /* IF the track is downloadable */
                if (track.GetElementsByTagName("downloadable")[0].InnerText == "true")
                {
                    /* Grab it's url and ID */
                    string download = track.GetElementsByTagName("download-url")[0].InnerText;
                    string id = track.GetElementsByTagName("id")[0].InnerText;
                    /* Add to our KVP array of songs */
                    Console.WriteLine("ID: " + id + " Link: " + download);
                    songs.Add(id, download);
                }
            }
            /* Return key-value dictionary of songs */
            return songs;
        }

    }
}
