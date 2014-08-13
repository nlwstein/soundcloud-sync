using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/* Inherit the following namespace to be able to get the current working directory from Windows */
using System.IO;
/* Inherit the following namespace to bring in HTTPClient support */
using System.Net.Http;
using System.Threading;
namespace soundcloud_sync
{

    /* This class takes a dictionary and downloads it. Simple stuff. */
    class Downloader
    {
        public void DownloadCollection(Dictionary<Guid, Tuple<SCAPI.DownloadType, String, String>> Collection, String path)
        {
            /* If path parameter is null, just set it to the executable's location */
            if (path == null) { path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location); }
            /* If path does not exist, create it */
            Directory.CreateDirectory(path);
            /* For each track as key value pair in the passed collection */
            Collection.ToList<KeyValuePair<Guid, Tuple<SCAPI.DownloadType, String, String>>>().ForEach(t =>
            {
                String file = Path.Combine(path, t.Value.Item2 + ".mp3");
                byte[] fileContents;

                switch (File.Exists(file))
                {
                    case true:
                        Console.WriteLine("File already present, skipping.");
                        break;
                    case false:
                        switch (t.Value.Item1)
                        {
                            case SCAPI.DownloadType.Custom:
                                fileContents = DownloadItem(new Uri(t.Value.Item3),t.Value.Item2).Result;
                                File.WriteAllBytes(file, fileContents);
                                break;

                            case SCAPI.DownloadType.Native:
                                /* Async grab the data as a byte array. We need our client ID here again... */
                                fileContents = DownloadItem(new Uri(t.Value.Item3 + "?client_id=65466a9abd127b6123e9415731d67e3d"),t.Value.Item2).Result;
                                /* Write all the bytes to a file that is the root path + the id as file name */
                                File.WriteAllBytes(file, fileContents);
                                break;
                        }
                        break;

                }

            });
        }

        private async Task<byte[]> DownloadItem(Uri uri, string SCID)
        {
            using (HttpClient client = new HttpClient())
            {
                Console.WriteLine("Downloading {0}...",SCID);
                bool _success = false;
                int failCounter = 0;
                while (_success == false && failCounter < 10)
                {
                    try
                    {
                        var result = await client.GetByteArrayAsync(uri);
                        _success = true;
                        return result;
                    }
                    catch
                    {
                        failCounter++;
                        Console.WriteLine("Download failed, trying again...");
                        Thread.Sleep(1000);
                        continue;
                    }
                }
                return null;
            }

        }

    }

}
