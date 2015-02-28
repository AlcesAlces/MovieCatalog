using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MovieCatalog.Helper_Functions
{
    static class ImageHandler
    {
        /// <summary>
        /// Async method for getting the IMG bytes file.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private static async Task<byte[]> getContentAsync(string url)
        {
            var content = new MemoryStream();

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);

            using (WebResponse response = await webRequest.GetResponseAsync())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    await responseStream.CopyToAsync(content);
                }
            }

            return content.ToArray();
        }

        private static BitmapImage getImageByBytes(byte[] toReturn)
        {

            BitmapImage imageToReturn = new BitmapImage();

            try
            {
                MemoryStream stream = new MemoryStream(toReturn);
                imageToReturn.BeginInit();
                imageToReturn.StreamSource = stream;
                imageToReturn.DecodePixelWidth = 200;
                imageToReturn.EndInit();
            }

            catch(Exception ex)
            {

            }

            return imageToReturn;
        }

        public static async Task<BitmapImage> bitmapFromUrl(string url)
        {
            return getImageByBytes(await getContentAsync(url));
        }

    }
}
