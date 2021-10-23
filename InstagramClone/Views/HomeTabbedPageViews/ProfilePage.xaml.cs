using InstagramClone.Views.LoginPageViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InstagramClone.Views.HomeTabbedPageViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage()
        {
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {            
            Preferences.Set("FirebaseRefreshToken", null);
            NavigationPage loginPage = new NavigationPage(new LoginPage());
            NavigationPage.SetHasNavigationBar(loginPage, false);
            Navigation.PushAsync(loginPage);
        }
    }
}