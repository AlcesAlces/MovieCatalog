﻿using MovieCatalog.Helper_Functions;
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

    //API KEY: 56587e13dc926d742e62c09151418bd3

    /// <summary>
    /// Interaction logic for PageControl.xaml
    /// </summary>
    public partial class PageControl : UserControl
    {
        string moviePosterPath = "https://image.tmdb.org/t/p/w396";
        ObservableCollection<Movie> _MovieCollection = Global._MovieCollection;
        public BitmapImage _image; 

        public PageControl()
        {
            initDefaultValues();
            InitializeComponent();
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
        /// Databinding for the i
        /// </summary>
        public BitmapImage ImageDisplay
        {
            get
            {
                var nullTest = lvMovies.SelectedItem;

                if(nullTest != null)
                {
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(moviePosterPath + ((Movie)lvMovies.SelectedItem).imageLocation, UriKind.Absolute);
                    bitmap.EndInit();
                    return bitmap;
                }

                else if (_MovieCollection.Count != 0)
                {
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(moviePosterPath + _MovieCollection[0].imageLocation, UriKind.Absolute);
                    bitmap.EndInit();
                    return bitmap;
                }

                else
                {
                    return null;
                }
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

        private void lvMovies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            imgPoster.Source = ImageDisplay;
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

            if (MessageBox.Show("Are you sure you want to delete these " +
                                lvMovies.SelectedItems.Count + " movies?","Confirm", MessageBoxButton.OKCancel)
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
                    foreach(Movie item in temp)
                    {
                        _MovieCollection.Add(item);
                    }
                }
            }
        }
    }
}
