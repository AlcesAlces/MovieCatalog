using MovieCatalog.HelperClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MovieCatalog.Helper_Functions
{
    static class FileUtility
    {
        public static List<Movie> loadMovieDataFromXML()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(@"XMLSettings\UserSettings.xml");

            XmlNodeList nodes = doc.DocumentElement.SelectNodes("/creatures/creature");

            List<Movie> toReturn = new List<Movie>();

            //Parse the xml file out
            foreach (XmlNode node in nodes)
            {
                toReturn.Add(new Movie
                {
                    //imageLocation = node.SelectSingleNode("image").ToString(),
                    //name = node.SelectSingleNode("name").ToString(),
                    //year = Int32.Parse(node.SelectSingleNode("year").ToString())
                });
            }

            return toReturn;
        }
    }
}
