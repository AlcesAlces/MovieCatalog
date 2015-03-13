using MovieCatalog.Helper_Functions;
using MovieCatalog.HelperClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WatTmdb.V3;

namespace MovieCatalog.Pages
{
    /// <summary>
    /// Interaction logic for SearchPage.xaml
    /// </summary>
    public partial class SearchPage : UserControl
    {
        private readonly string apikey = "56587e13dc926d742e62c09151418bd3";
        private readonly string language = "en";
        ObservableCollection<Movie> _MovieCollection = new ObservableCollection<Movie>();
        TmdbMovie selectedMovie = null;

        public SearchPage()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            search();
        }

        private void search()
        {
            _MovieCollection.Clear();

            string searchString = txtSearch.Text;
            var movies = TMDBHelper.movieResultsBySearch(searchString);
            foreach (MovieResult item in movies)
            {
                _MovieCollection.Add(new Movie()
                {
                    name = item.title,
                    year = item.release_date.ToString(),
                    mid = item.id
                });
            }

            //Set the currently selected movie to the first element.
            if(movies.Count != 0)
            {
                selectedMovie = TMDBHelper.getTmdbMovieById(movies[0].id);
            }

            applyImageResults();
        }

        public ObservableCollection<Movie> MovieCollection
        {
            get { return _MovieCollection; }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            bool success = true;
            int successAmt = 0;
            bool duplicate = false;
            int duplicateAmt = 0;

            foreach(Movie item in lvResults.SelectedItems)
            {
                TmdbMovie movie = TMDBHelper.getTmdbMovieById(item.mid);
                TmdbMovieImages image = TMDBHelper.getImagesById(item.mid);

                try
                {
                    if (FileHandlers.isMovieDuplicate(movie.id))
                    {
                        //MessageBox.Show(movie.title + " is already in the xml file!");
                        duplicateAmt++;
                        duplicate = true;
                    }

                    else
                    {
                        string posterLocation = "";

                        try
                        {
                            posterLocation = image.posters[0].file_path;
                        }

                        catch (Exception ex)
                        {
                            posterLocation = "NONE";
                        }

                        FileHandlers.addMovie(new Movie
                        {
                            description = movie.overview,
                            imageLocation = posterLocation,
                            mid = movie.id,
                            name = movie.title,
                            onlineRating = movie.vote_average,
                            userRating = 0.0,
                            year = movie.release_date,
                            genres = movie.genres
                        });
                        successAmt++;
                    }
                }
                catch(Exception ex)
                {
                    success = false;
                }
            }

            //Refresh the movie list
            List<Movie> movies = FileHandlers.allMoviesInXml();
            Global._MovieCollection.Clear();
            foreach(Movie movie in movies)
            {
                Global._MovieCollection.Add(movie);
            }

            string displayString = success ? "Successfully Added:\n" : "Unsuccessful addition of movies!";
            displayString += success ? successAmt + " movies added\n" : "0 movies added";
            displayString += duplicate ? duplicateAmt + " duplicates not added" : "0 duplicates found";

            MessageBox.Show(displayString, "Results");
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                search();
            }
        }

        private void lvResults_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            applyImageResults();
            txtDescription.Text = Description;
            lblTitle.Content = TitleDisplay;
        }

        //Assign the appropriate image to the display image based on selection.
        private void applyImageResults()
        {
            if (lvResults.Items.Count > 0)
            {
                Movie movieId = ((Movie)lvResults.SelectedItem);
                Tmdb connection = new Tmdb(apikey, language);
                TmdbMovieImages images;

                //An element in the list is selected, no need to recalcuate
                if (movieId != null)
                {
                    selectedMovie = connection.GetMovieInfo(movieId.mid);
                    images = connection.GetMovieImages(movieId.mid);
                    setImage(images);
                }
                //An element in the list is not selected, recalculate setting element 0
                else
                {
                    movieId = ((Movie)lvResults.Items[0]);
                    selectedMovie = connection.GetMovieInfo(movieId.mid);
                    images = connection.GetMovieImages(movieId.mid);

                    setImage(images);

                }
            }
        }

        /// <summary>
        /// Set display image based on the posters found from the database.
        /// </summary>
        /// <param name="images"></param>
        private void setImage(TmdbMovieImages images)
        {
            if (images.posters.Count() == 0)
            {
                imageSearch.Source = genericImage();
            }

            else
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(Global.moviePosterPath + images.posters[0].file_path, UriKind.Absolute);
                bitmap.EndInit();
                imageSearch.Source = bitmap;
            }
        }

        /// <summary>
        /// Return the generic image.
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

        #region databindings

        public string Description
        {
            get
            {
                if (selectedMovie != null)
                {
                    return selectedMovie.overview;
                }
                else
                {
                    return "";
                }
            }
        }

        public string TitleDisplay
        {
            get
            {
                if(selectedMovie != null)
                {
                    return selectedMovie.title;
                }

                else
                {
                    return "";
                }
            }
        }

        #endregion

    }
}
