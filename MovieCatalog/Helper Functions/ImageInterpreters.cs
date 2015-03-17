using MovieCatalogLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MovieCatalog.Helper_Functions
{
    static class ImageInterpreters
    {

        private static ImageHandler imageHandler = new ImageHandler();

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

            catch (Exception ex)
            {

            }

            return imageToReturn;
        }

        public static async Task<BitmapImage> bitmapFromUrl(string url)
        {
            return getImageByBytes(await imageHandler.getContentAsync(url));
        }

        /// <summary>
        /// Databinding for the image
        /// </summary>
        public static async Task<BitmapImage> ImageDisplay(ObservableCollection<Movie> _MovieCollection, Movie selectedItem)
        {
            if (selectedItem != null)
            {
                if (selectedItem.imageLocation == "NONE")
                {
                    return genericImage();
                }

                else
                {

                    return await bitmapFromUrl(Global.moviePosterPath + selectedItem.imageLocation);
                }
            }

            else if (_MovieCollection.Count != 0)
            {
                if (_MovieCollection[0].imageLocation == "NONE")
                {
                    return genericImage();
                }
                else
                {

                    return await bitmapFromUrl(Global.moviePosterPath + _MovieCollection[0].imageLocation);
                }
            }

            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns a generic image to put in the image box.
        /// </summary>
        /// <returns></returns>
        public static BitmapImage genericImage()
        {
            using (var memory = new MemoryStream())
            {
                MovieCatalog.Properties.Resources._5iRXRbX4T.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                return bitmapImage;
            }
        }
    }
}
