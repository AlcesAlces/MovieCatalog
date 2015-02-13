using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatTmdb.V3;

namespace MovieCatalog.HelperClasses
{
    public class Movie
    {
        public string name { get; set; }
        public string year { get; set; }
        public int mid { get; set; }
        public string imageLocation { get; set; }
        public double onlineRating { get; set; }
        public double userRating { get; set; }
        public string description { get; set; }
        public List<MovieGenre> genres = new List<MovieGenre>();

        public Movie(string toParse)
        {
            setGenreCommaSeperated(toParse);
        }

        public Movie()
        {

        }

        public string getGenreCommaSeperated()
        {
            string toReturn = "";

            foreach(MovieGenre genre in genres)
            {
                toReturn += genre.name + ",";
            }

            return toReturn.Substring(0, toReturn.Count() - 1);
        }

        public void setGenreCommaSeperated(string toBeParsed)
        {
            List<string> splitString = toBeParsed.Split(',').ToList();
            
            foreach(string item in splitString)
            {
                genres.Add(new MovieGenre
                    {
                        name = item
                    });
            }
        }
    }
}
