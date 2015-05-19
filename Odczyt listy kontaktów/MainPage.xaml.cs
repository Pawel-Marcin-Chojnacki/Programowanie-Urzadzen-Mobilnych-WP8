using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Windows.Phone.PersonalInformation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.UserData;
using Odczyt_listy_kontaktów.Resources;
using Microsoft.Phone.UserData;
using Microsoft.Phone.Tasks;

namespace Odczyt_listy_kontaktów
{


    public partial class MainPage : PhoneApplicationPage
    {

        private SaveContactTask saveContact;
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            saveContact = new SaveContactTask();
            saveContact.Completed += new EventHandler<SaveContactResult>(saveContact_Completed);
            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        private void saveContact_Completed(object sender, SaveContactResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                ContactSavedStatus.Text = "Contact saved.";
                MessageBox.Show("Saved OK");
            }
        }

        private void wczytaj_kontakty_Click(object sender, RoutedEventArgs e)
        {
            Contacts cons = new Contacts();

            IEnumerable<Account> accounts = cons.Accounts;
            cons.SearchCompleted += cons_SearchCompleted;
            cons.SearchAsync(String.Empty, FilterKind.None, null);
        }

        void cons_SearchCompleted(object sender, Microsoft.Phone.UserData.ContactsSearchEventArgs e)
        {
            try
            {
                ContactResultsData.DataContext = e.Results;
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }

        private void DodajKontakt_Click(object sender, RoutedEventArgs e)
        {
            saveContact.FirstName = FirstNameTextBox.Text;
            saveContact.LastName = LastNameTextBox.Text;
            saveContact.Show();
        }

        async private void UtworzNowyKontakt_Click(object sender, RoutedEventArgs e)
        {
            ContactStore store = await ContactStore.CreateOrOpenAsync();

            StoredContact contact = new StoredContact(store)
            {
                RemoteId = Guid.NewGuid().ToString(),
                GivenName = FirstNameTextBox.Text,
                FamilyName = LastNameTextBox.Text,
            };

            IDictionary<string, object> props = await contact.GetExtendedPropertiesAsync();
            props.Add("Password", PasswordEntry.Text);
            await contact.SaveAsync();

        }


        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}