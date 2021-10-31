using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using InstagramClone.Views.InteractionViews;
using InstagramClone.Views.LoginPageViews;
using Xamarin.Essentials;

namespace InstagramClone
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            //if (string.IsNullOrEmpty(Preferences.Get("FirebaseRefreshToken", "")))
            //{
            //    MainPage = new NavigationPage(new LoginPage());
            //}
            //else
            //{
            //    MainPage = new NavigationPage(new HomeTabbedPage());
            //}
            MainPage = new NavigationPage(new CommentPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
