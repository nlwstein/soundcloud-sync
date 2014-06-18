using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/* Inherit the following namespace to be able to get the current working directory from Windows */
using System.IO;
/* Inherit the following namespace to bring in HTTPClient support */
using System.Net.Http;

namespace soundcloud_sync
{

    /* This class takes a dictionary and downloads it. Simple stuff. */
    class Downloader
    {
        public void DownloadCollection(Dictionary<String, String> collection, String path)
        {
            /* If path parameter is null, just set it to the executable's location */
            if (path == null) { path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location); }
            /* If path does not exist, create it */
            Directory.CreateDirectory(path);
            /* For each track as key value pair in the passed collection */
            foreach (KeyValuePair<String, String> track in collection)
            {
                /* Let's define what the file's name will eventually be */
                String file = path + @"\" + track.Key + ".mp3";
                if (!File.Exists(file))
                {
                    /* Async grab the data as a byte array. We need our client ID here again... */
                    var fileValue = DownloadItem(new Uri(track.Value + "?client_id=65466a9abd127b6123e9415731d67e3d")).Result;
                    /* Write all the bytes to a file that is the root path + the id as file name */
                    File.WriteAllBytes(file, fileValue);
                    Console.WriteLine("Saved.");
                }
                else
                {
                    Console.WriteLine("File already present, skipping.");
                }

            }
        }
        private async Task<byte[]> DownloadItem(Uri uri)
        {
            using (HttpClient client = new HttpClient())
            {
                Console.WriteLine("Downloading...");
                var result = await client.GetByteArrayAsync(uri);
                return result;
            }

        }

    }

}
