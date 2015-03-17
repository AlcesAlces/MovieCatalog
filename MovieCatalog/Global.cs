using MovieCatalogLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCatalog
{
    static class Global
    {
        public static ObservableCollection<Movie> _MovieCollection = new ObservableCollection<Movie>();
        public static string moviePosterPath = "https://image.tmdb.org/t/p/w396";
    }
}
