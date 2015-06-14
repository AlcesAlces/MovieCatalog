using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using SocketIOClient;
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
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : UserControl
    {
        //TODO: Adjust dbeug statement
        bool _debug = true;

        public LoginPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Moderatly janky function which will verify with the DB that a user can be created.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            login();
        }

        private async void login()
        {
            //TODO: Replace this commented code.
            //var checkUser = await MovieCatalogLibrary.DatabaseHandling.MongoInteraction.UserIdByName(tbUsername.Text);

            //if (checkUser == null)
            //{
            //    MessageBox.Show("Username does not exist");
            //}

            //else
            //{
            Global.socket = new Client("http://127.0.0.1:8000/");

            Global.socket.On("connect", (fn) =>
            {
                
            });

            Global.socket.Connect();

            //TODO: Add timeout code.
            while (!Global.socket.IsConnected) ;

            MovieCatalogLibrary.DatabaseHandling.PasswordHash pwHash = new MovieCatalogLibrary.DatabaseHandling.PasswordHash();

            if (await MovieCatalogLibrary.DatabaseHandling.MongoInteraction.VerifyCredentials(tbUsername.Text, pwHash.CreateHashNonStatic(tbPassword.Password), Global.socket) || _debug)
                {
                    //User is logged in
                    Global.userName = tbUsername.Text;
                //TODO: Change this hardcoding
                    Global.uid = "5550fc3dc1ec151d044a83b3";

                    Global.userLink.DisplayName = Global.userName;
                    Global.userLink.Source = new Uri("/Pages/UserSettingsPage.xaml", UriKind.Relative);

                    if(await MovieCatalogLibrary.DatabaseHandling.MongoInteraction.GetUserSyncStatus(Global.userName))
                    {
                        //TODO: uncoment this and fix it too lol
                        //MovieCatalogLibrary.DatabaseHandling.MongoXmlLinker.SyncUserFiles(Global.uid);
                    }

                    //Don't automatically update.
                    NavigationCommands.GoToPage.Execute("/Pages/UserSettingsPage.xaml", this);
                }

                else
                {
                    //Incorrect information
                    MessageBox.Show("The information you have have entered is incorrect");
                }
            //}
        }

        /// <summary>
        /// Another moderately janky function which chekcs to see if the user can register.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            Global.socket = new Client("http://127.0.0.1:8000/");

            //TODO: Uncomment this
            //var checkUser = await MovieCatalogLibrary.DatabaseHandling.MongoInteraction.UserIdByName(tbUsername.Text);
            var checkUser = "55513ce59b71ef1088a2115a";

            if(checkUser == null)
            {
                await MovieCatalogLibrary.DatabaseHandling.MongoInteraction.CreateUser(tbUsername.Text, tbPassword.Password, Global.socket);
                MessageBox.Show("User created successfully! Please log in");
                tbUsername.Text = "";
                tbPassword.Password = "";
            }

            else
            {
                MessageBox.Show("Username is already taken! Try again");
            }
        }

        private void completionListener_Event(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                login();
            }
        }
    }
}
