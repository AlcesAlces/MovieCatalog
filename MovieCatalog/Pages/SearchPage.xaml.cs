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

        public SearchPage()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            _MovieCollection.Clear();

            string searchString = txtSearch.Text;
            Tmdb api = new Tmdb(apikey, language);
            TmdbMovieSearch test = api.SearchMovie(searchString, 1);

            foreach(MovieResult item in test.results)
            {
                _MovieCollection.Add(new Movie()
                    {
                        name = item.title,
                        year = item.release_date.ToString(),
                        mid = item.id
                    });
            }

            //TmdbMovie movie = api.GetMovieInfo(test.results[0].id, language);
        }

        public ObservableCollection<Movie> MovieCollection
        {
            get { return _MovieCollection; }
        }
    }
}
