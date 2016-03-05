using MovieCatalogLibrary;
using MovieCatalogLibrary.DatabaseHandling;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using System.Windows.Threading;

namespace MovieCatalog.Pages
{
    /// <summary>
    /// Interaction logic for UserSettingsPage.xaml
    /// </summary>
    public  partial class UserSettingsPage : UserControl
    {
        public UserSettingsPage()
        {
            //Load();
            InitializeComponent();
        }

        private async void Load()
        {
            if (await MovieCatalogLibrary.DatabaseHandling.MongoInteraction.GetUserSyncStatus(Global.userName))
            {
                rbAutomatic.IsChecked = true;
                rbManual.IsChecked = false;
            }

            else
            {
                rbManual.IsChecked = true;
                rbAutomatic.IsChecked = false;
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {

            //MessageBox.Show("This is only a test...");

            //SyncHelper sh = new SyncHelper(Global.socket);

            //sh.StartSync();

            //MessageBox.Show("This may take a minute...");
            try
            {
                BackgroundWorker bw = new BackgroundWorker();
                btnSync.IsEnabled = false;
                bw.DoWork += (object sender2, DoWorkEventArgs e2) =>
                {
                    MovieCatalogLibrary.DatabaseHandling.MongoXmlLinker.SyncUserFiles(Global.uid, Global.socket);
                    FileHandler fh = new FileHandler();
                    ObservableCollection<Movie> tempOc = new ObservableCollection<Movie>();
                    Dispatcher.BeginInvoke(new Action(() => fh.allMoviesInXml().ForEach(x => Global.test.MovieCollection.Add(x)) ));
                    
                    
                };

                bw.RunWorkerCompleted += (object sender2, RunWorkerCompletedEventArgs e2) =>
                {
                    btnSync.IsEnabled = true;
                };

                bw.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                //TODO: more elegant way of displaying this.
                MessageBox.Show(ex.Message);
            }

            FileHandler handler = new FileHandler();

            Global._MovieCollection.Clear();
            handler.allMoviesInXml().ForEach(x => Global._MovieCollection.Add(x));
        }

        private void rbAutomatic_Checked(object sender, RoutedEventArgs e)
        {
            MovieCatalogLibrary.DatabaseHandling.MongoInteraction.UpdateUser(Global.userName, true, Global.socket);
        }

        private void rbManual_Checked(object sender, RoutedEventArgs e)
        {
            MovieCatalogLibrary.DatabaseHandling.MongoInteraction.UpdateUser(Global.userName, false, Global.socket);
        }
    }
}
