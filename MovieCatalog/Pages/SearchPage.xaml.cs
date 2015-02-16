using MovieCatalog.Helper_Functions;
using MovieCatalog.HelperClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            Tmdb api = new Tmdb(apikey, language);
            TmdbMovieSearch test = api.SearchMovie(searchString, 1);

            foreach (MovieResult item in test.results)
            {
                _MovieCollection.Add(new Movie()
                {
                    name = item.title,
                    year = item.release_date.ToString(),
                    mid = item.id
                });
            }

            //Set the currently selected movie to the first element.
            if(test.results.Count != 0)
            {
                selectedMovie = api.GetMovieInfo(test.results[0].id);
            }

        }

        public ObservableCollection<Movie> MovieCollection
        {
            get { return _MovieCollection; }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Tmdb query = new Tmdb(apikey, language);

            bool success = true;
            int successAmt = 0;
            bool duplicate = false;
            int duplicateAmt = 0;

            foreach(Movie item in lvResults.SelectedItems)
            {
                TmdbMovie movie = query.GetMovieInfo(item.mid);
                TmdbMovieImages image = query.GetMovieImages(movie.id);

                try
                {
                    if (FileHandlers.isMovieDuplicate(movie.id))
                    {
                        MessageBox.Show(movie.title + " is already in the xml file!");
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

            Movie movieId = ((Movie)lvResults.SelectedItem);

            if(movieId != null)
            {
                Tmdb connection = new Tmdb(apikey, language);
                selectedMovie = connection.GetMovieInfo(movieId.mid);
                TmdbMovieImages images = connection.GetMovieImages(movieId.mid);

                try
                {

                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(Global.moviePosterPath + images.posters[0].file_path, UriKind.Absolute);
                    bitmap.EndInit();
                    imageSearch.Source = bitmap;
                }
                catch(Exception ex)
                {

                }
            }

            txtDescription.Text = Description;
            lblTitle.Content = TitleDisplay;
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
