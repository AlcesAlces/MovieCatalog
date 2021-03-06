﻿using MovieCatalog.Helper_Functions;
using MovieCatalogLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
        string displayString = "YOUR CATALOG (" + Global._MovieCollection.Count + ")";
        BitmapImage image = new BitmapImage();
        public FileHandler fileHandler = new FileHandler(MovieCatalogLibrary.FileHandler.platformType.Windows);


        public PageControl()
        {
            Global.test = this;
            initDefaultValues();
            InitializeComponent();
            lblOnlineRating.Content = OnlineRatingDisplay;
            txtDescription.Text = DescriptionDisplay;
            tbGenres.Text = GenresDisplay;
            tbCatalog.Text = "YOUR CATALOG (" + Global._MovieCollection.Count + ")";
            Global._MovieCollection.CollectionChanged += _MovieCollection_CollectionChanged;
        }

        /// <summary>
        /// Brings all of the information in from the xml and applies it
        /// to the components.
        /// </summary>
        private void initDefaultValues()
        {
            Global._MovieCollection.Clear();
            List<Movie> listOfMovies = fileHandler.allMoviesInXml();

            if(listOfMovies.Count == 0)
            { 

            }

            else
            {
                foreach(Movie movie in listOfMovies)
                {
                    Global._MovieCollection.Add(movie);
                }
            }
        }

        /// <summary>
        /// Databinding for the movie list box.
        /// </summary>
        public ObservableCollection<Movie> MovieCollection
        {
            get { return Global._MovieCollection; }
            set { Global._MovieCollection = value; }
        }

        public string CatalogDisplay
        {
            get { return displayString; }

            set
            {
                value = displayString;
            }

        }

        private string OnlineRatingDisplay
        {
            get
            {
                if (Global._MovieCollection.Count == 0)
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
                        return "Rating: " + Global._MovieCollection[0].onlineRating + "/10";
                    }
                }
            }
        }

        public string DescriptionDisplay
        {
            get
            {
                if (Global._MovieCollection.Count == 0)
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
                        return Global._MovieCollection[0].description;
                    }
                }
            }
        }

        public string GenresDisplay
        {
            get
            {
                if (Global._MovieCollection.Count == 0)
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
                        return Global._MovieCollection[0].getGenreCommaSeperated();
                    }
                }
            }
        }

        private async void lvMovies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await setImageContent();
            lblOnlineRating.Content = OnlineRatingDisplay;
            txtDescription.Text = DescriptionDisplay;
            tbGenres.Text = GenresDisplay;
        }

        private async Task setImageContent()
        {
            imgPoster.Source = ImageInterpreters.genericImage();
            imgPoster.Source = await ImageInterpreters.ImageDisplay(Global._MovieCollection, lvMovies.SelectedItem as Movie);
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
                foreach (Movie item in Global._MovieCollection)
                {
                    toSend.Add(item);
                }

                List<Movie> movies = Filtering.filterByName(nameFilter, toSend);

                Global._MovieCollection.Clear();

                foreach(Movie item in movies)
                {
                    Global._MovieCollection.Add(item);
                }
            }

        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            List<Movie> movies = fileHandler.allMoviesInXml();

            Global._MovieCollection.Clear();

            foreach (Movie item in movies)
            {
                Global._MovieCollection.Add(item);
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

        private async void removeMovies()
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

                    if (Global.uid == null)
                    {
                        fileHandler.removeMovies(movies);
                    }

                    else
                    {
                        await MovieCatalogLibrary.DatabaseHandling.MongoXmlLinker.RemoveMovies(movies,Global.uid, Global.socket);
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    List<Movie> temp = fileHandler.allMoviesInXml();
                    Global._MovieCollection.Clear();
                    foreach (Movie item in temp)
                    {
                        Global._MovieCollection.Add(item);
                    }
                }
            }
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            await setImageContent();
        }

        private void _MovieCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            tbCatalog.Text = "YOUR CATALOG (" + Global._MovieCollection.Count + ")";
        }
    }
}
