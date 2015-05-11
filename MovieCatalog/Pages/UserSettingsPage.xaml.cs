using MovieCatalogLibrary;
using System;
using System.Collections.Generic;
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

namespace MovieCatalog.Pages
{
    /// <summary>
    /// Interaction logic for UserSettingsPage.xaml
    /// </summary>
    public  partial class UserSettingsPage : UserControl
    {
        public UserSettingsPage()
        {
            Load();
            InitializeComponent();
        }

        private async void Load()
        {
            if (await MovieCatalogLibrary.DatabaseHandling.MongoInteraction.GetUserSyncStatus(Global.userName))
            {
                rbAutomatic.IsChecked = true;
            }

            else
            {
                rbManual.IsChecked = true;
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("This may take a minute...");
            await MovieCatalogLibrary.DatabaseHandling.MongoXmlLinker.SyncUserFiles(Global.uid);

            FileHandler handler = new FileHandler();

            Global._MovieCollection.Clear();
            handler.allMoviesInXml().ForEach(x => Global._MovieCollection.Add(x));
        }

        private void rbAutomatic_Checked(object sender, RoutedEventArgs e)
        {
            MovieCatalogLibrary.DatabaseHandling.MongoInteraction.UpdateUser(Global.userName, true);
        }

        private void rbManual_Checked(object sender, RoutedEventArgs e)
        {
            MovieCatalogLibrary.DatabaseHandling.MongoInteraction.UpdateUser(Global.userName, false);
        }
    }
}
