using FirstFloor.ModernUI.Presentation;
using MovieCatalogLibrary;
using SocketIOClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MovieCatalog
{
    static class Global
    {
        public static ObservableCollection<Movie> _MovieCollection = new ObservableCollection<Movie>();
        public static string moviePosterPath = "https://image.tmdb.org/t/p/w396";
        public static string userName = null;
        public static string uid = null;
        public static Link userLink = null;
        public static Client socket;
        public static int serverTimeout = 10000;
        public static string connectionString = "http://23.96.28.16:80/";
        //public static string connectionString = "http://127.0.0.1:80/";
    }
}
