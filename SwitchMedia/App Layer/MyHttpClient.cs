using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Switch.Core;
using System.Xml;
using System.Drawing;
using Android.Graphics;

namespace SwitchMedia.App_Layer
{
    public class MyHttpClient :IMyHttpClient
    {
        private const string URL_PATTERN = "http://www.colourlovers.com/api/patterns/random";
        private const string URL_COLOR = "http://www.colourlovers.com/api/colors/random";

        public DPattern DownloadPattern()
        {
            return fetchPattern(URL_PATTERN);
        }

        public DPattern DownloadColor()
        {
            
            return fetchColor(URL_COLOR);
        }

        private DPattern fetchColor(string url)
        {
            DPattern pattern = new DPattern(DPatternType.Color, "", null, 0);
            // Create an HTTP web request using the URL:
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.ContentType = "application/xml";
            request.Method = "GET";

            // Send the request to the server and wait for the response:
            using (WebResponse response = request.GetResponse())
            {
                // Get a stream representation of the HTTP web response:
                using (Stream stream = response.GetResponseStream())
                {
                    using (XmlReader reader = XmlReader.Create(stream))
                    {
                        int startReading = 0;
                        int color=0;
                        while (reader.Read())
                        {
                            switch (reader.NodeType)
                            {
                                case XmlNodeType.Element:
                                    if (reader.Name == "red")
                                        startReading = 256*256;
                                    break;
                                case XmlNodeType.Text:
                                    if (startReading > 0)
                                    {
                                        color += startReading * Convert.ToInt32(reader.Value);
                                        startReading /= 256;
                                        if (startReading == 0) startReading=-1;
                                    }

                                    break;                                
                            }
                            if (startReading == -1) break;
                        }
                        pattern.Color = color;
                    }
                }
            }
            return pattern;
        }

        private DPattern fetchPattern(string url)
        {
            string rawUrl = "";
            DPattern pattern = new DPattern(DPatternType.Image, "", null, 0);
            // Create an HTTP web request using the URL:
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.ContentType = "application/xml";
            request.Method = "GET";

            // Send the request to the server and wait for the response:
            using (WebResponse response = request.GetResponse())
            {
                // Get a stream representation of the HTTP web response:
                using (Stream stream = response.GetResponseStream())
                {
                    using (XmlReader reader = XmlReader.Create(stream))
                    {
                        int startReading = 0;
                        while (reader.Read())
                        {
                            switch (reader.NodeType)
                            {
                                case XmlNodeType.Element:
                                    if (reader.Name == "imageUrl")
                                        startReading = 1;
                                    break;
                                case XmlNodeType.CDATA:
                                    if (startReading > 0)
                                    {
                                        rawUrl = reader.Value;
                                        startReading =-1;
                                    }

                                    break;

                            }
                            if (startReading == -1) break;
                        }
                        //rawUrl = getURL(rawUrl);
                    }
                }
            }


            WebClient webClient = new WebClient();
            byte[] bytes = null;
            bytes = webClient.DownloadData(new Uri(rawUrl));

            Bitmap bm = BitmapFactory.DecodeByteArray(bytes, 0, bytes.Length);
            pattern.Image = bm;

            return pattern;
        }

        private string getURL(string rawUrl)
        {

            return rawUrl.Substring(8, rawUrl.Length - 11);
        }

    }
}