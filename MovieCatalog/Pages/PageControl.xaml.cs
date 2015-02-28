using MovieCatalog.Helper_Functions;
using MovieCatalog.HelperClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WatTmdb.V3;

namespace MovieCatalog.Pages
{

    //API KEY: 56587e13dc926d742e62c09151418bd3

    /// <summary>
    /// Interaction logic for PageControl.xaml
    /// </summary>
    public partial class PageControl : UserControl
    {
        ObservableCollection<Movie> _MovieCollection = Global._MovieCollection;
        BitmapImage image = new BitmapImage();

        public PageControl()
        {
            initDefaultValues();
            InitializeComponent();
            lblOnlineRating.Content = OnlineRatingDisplay;
            txtDescription.Text = DescriptionDisplay;
            tbGenres.Text = GenresDisplay;
        }

        /// <summary>
        /// Brings all of the information in from the xml and applies it
        /// to the components.
        /// </summary>
        private void initDefaultValues()
        {
            _MovieCollection.Clear();
            List<Movie> listOfMovies = FileHandlers.allMoviesInXml();

            if(listOfMovies.Count == 0)
            { 

            }

            else
            {
                foreach(Movie movie in listOfMovies)
                {
                    _MovieCollection.Add(movie);
                }
            }
        }

        /// <summary>
        /// Databinding for the movie list box.
        /// </summary>
        public ObservableCollection<Movie> MovieCollection
        {
            get { return _MovieCollection; }
        }

        /// <summary>
        /// Databinding for the image
        /// </summary>
        public async Task<BitmapImage> ImageDisplay()
        {
            var nullTest = lvMovies.SelectedItem;

            if (nullTest != null)
            {
                if (((Movie)lvMovies.SelectedItem).imageLocation == "NONE")
                {
                    return genericImage();
                }

                else
                {

                    return await ImageHandler.bitmapFromUrl(Global.moviePosterPath + ((Movie)lvMovies.SelectedItem).imageLocation);
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
                    
                    return await ImageHandler.bitmapFromUrl(Global.moviePosterPath + _MovieCollection[0].imageLocation);
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
        private BitmapImage genericImage()
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

        private string OnlineRatingDisplay
        {
            get
            {
                if(_MovieCollection.Count == 0)
                {
                    return "N/A";
                }

                else
                {
                    var nullTest = lvMovies.SelectedItem;

                    if (nullTest != null)
                    {
                        return "Rating: " + ((Movie)lvMovies.SelectedItem).onlineRating.ToString() + "/10";
                    }
                    else
                    {
                        return "Rating: " + _MovieCollection[0].onlineRating + "/10";
                    }
                }
            }
        }

        public string DescriptionDisplay
        {
            get
            {
                if (_MovieCollection.Count == 0)
                {
                    return "N/A";
                }

                else
                {
                    var nullTest = lvMovies.SelectedItem;

                    if (nullTest != null)
                    {
                        return "" + ((Movie)lvMovies.SelectedItem).description.ToString();
                    }
                    else
                    {
                        return _MovieCollection[0].description;
                    }
                }
            }
        }

        public string GenresDisplay
        {
            get
            {
                if (_MovieCollection.Count == 0)
                {
                    return "N/A";
                }

                else
                {
                    var nullTest = lvMovies.SelectedItem;

                    if (nullTest != null)
                    {
                        return "" + ((Movie)lvMovies.SelectedItem).getGenreCommaSeperated();
                    }
                    else
                    {
                        return _MovieCollection[0].getGenreCommaSeperated();
                    }
                }
            }
        }

        private async void lvMovies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            imgPoster.Source = await ImageDisplay();
            lblOnlineRating.Content = OnlineRatingDisplay;
            txtDescription.Text = DescriptionDisplay;
            tbGenres.Text = GenresDisplay;
        }

        private void btnFilter_Click(object sender, RoutedEventArgs e)
        {

            string nameFilter = "";

            //Don't apply the name filter, if name textbox is empty.
            if(tbName.Text.Count() == 0)
            {
                nameFilter = null;
            }

            else
            {
                nameFilter = tbName.Text.Trim().ToLower();
            }

            if(nameFilter != null)
            {

                List<Movie> toSend = new List<Movie>();
                foreach(Movie item in _MovieCollection)
                {
                    toSend.Add(item);
                }

                List<Movie> movies = Filtering.filterByName(nameFilter, toSend);

                _MovieCollection.Clear();

                foreach(Movie item in movies)
                {
                    _MovieCollection.Add(item);
                }
            }

        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            List<Movie> movies = FileHandlers.allMoviesInXml();

            _MovieCollection.Clear();

            foreach (Movie item in movies)
            {
                _MovieCollection.Add(item);
            }
            
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {

            removeMovies();
        }

        private void lvMovies_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Delete)
            {
                removeMovies();
            }
        }

        private void removeMovies()
        {
            if (MessageBox.Show("Are you sure you want to delete these " +
                                lvMovies.SelectedItems.Count + " movies?", "Confirm", MessageBoxButton.OKCancel)
                == MessageBoxResult.OK)
            {

                try
                {
                    List<Movie> movies = new List<Movie>();

                    foreach (Movie item in lvMovies.SelectedItems)
                    {
                        movies.Add(item);
                    }

                    FileHandlers.removeMovies(movies);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    List<Movie> temp = FileHandlers.allMoviesInXml();
                    _MovieCollection.Clear();
                    foreach (Movie item in temp)
                    {
                        _MovieCollection.Add(item);
                    }
                }
            }
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            imgPoster.Source = await ImageDisplay();
        }
    }
}
