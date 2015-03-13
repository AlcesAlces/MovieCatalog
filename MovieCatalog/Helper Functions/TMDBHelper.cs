using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatTmdb.V3;

namespace MovieCatalog.Helper_Functions
{
    /// <summary>
    /// Use this static class to interact with all TMDB functions provided by wattmdb
    /// </summary>
    static class TMDBHelper
    {
        private static readonly string apikey = "56587e13dc926d742e62c09151418bd3";
        private static readonly string language = "en";
        private static Tmdb api = new Tmdb(apikey, language);

        public static List<MovieResult> movieResultsBySearch(string searchTerm)
        {
            if (searchTerm.Count() != 0)
            {
                TmdbMovieSearch results = api.SearchMovie(searchTerm, 1);
                return (api.SearchMovie(searchTerm, 1) as TmdbMovieSearch).results;
            }

            else
            {
                return new List<MovieResult>();
            }
        }

        public static TmdbMovie getTmdbMovieById(int mid)
        {
            return api.GetMovieInfo(mid);
        }

        public static TmdbMovieImages getImagesById(int mid)
        {
            return api.GetMovieImages(mid);
        }
    }
}
