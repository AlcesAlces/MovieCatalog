using MovieCatalogLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCatalog.Helper_Functions
{
    static class Filtering
    {
        public static List<Movie> filterByName(string name, List<Movie> movies)
        {

            return         (from b in movies
                           where b.name.ToLower().Contains(name)
                           select b).ToList();
        }
    }
}
