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
            var checkUser = await MovieCatalogLibrary.DatabaseHandling.MongoInteraction.UserIdByName(tbUsername.Text);

            if (checkUser == null)
            {
                MessageBox.Show("Username does not exist");
            }

            else
            {
                if (await MovieCatalogLibrary.DatabaseHandling.MongoInteraction.VerifyCredentials(tbUsername.Text, tbPassword.Password))
                {
                    //User is logged in
                    Global.userName = tbUsername.Text;
                    Global.uid = checkUser;

                    Global.userLink.DisplayName = Global.userName;
                    Global.userLink.Source = new Uri("/Pages/UserSettingsPage.xaml", UriKind.Relative);

                    MovieCatalogLibrary.DatabaseHandling.MongoXmlLinker.SyncUserFiles(Global.uid);
                    NavigationCommands.GoToPage.Execute("/Pages/UserSettingsPage.xaml", this);
                }

                else
                {
                    //Incorrect information
                    MessageBox.Show("The information you have have entered is incorrect");
                }
            }
        }

        /// <summary>
        /// Another moderately janky function which chekcs to see if the user can register.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            var checkUser = await MovieCatalogLibrary.DatabaseHandling.MongoInteraction.UserIdByName(tbUsername.Text);

            if(checkUser == null)
            {
                await MovieCatalogLibrary.DatabaseHandling.MongoInteraction.CreateUser(tbUsername.Text, tbPassword.Password);
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
