﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using InstagramClone.Views.LoginPageViews;

using Firebase.Auth;
using Newtonsoft.Json;
using Xamarin.Essentials;
using InstagramClone.Models;

namespace InstagramClone.Views.LoginPageViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            LogoImage.Source = ImageSource.FromResource("InstagramClone.Resources.Images.InstagramLogo.svg.png");
        }

        async private void BtnLogIn_Clicked(object sender, EventArgs e)
        {

            var authProvider = FirebaseDB.GetAuthProvider();
            try
            {
                var auth = await authProvider.SignInWithEmailAndPasswordAsync(TxtUsername.Text, TxtPassword.Text);
                var content = await auth.GetFreshAuthAsync();
                var serializedContent = JsonConvert.SerializeObject(content);
                Preferences.Set("FirebaseRefreshToken", serializedContent);
                Preferences.Set("UID", auth.User.LocalId);
                FirebaseDB.CurrentUserId = auth.User.LocalId;
                await Navigation.PushAsync(new HomeTabbedPage());
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                await DisplayAlert("Alert", "Invalid email or password", "OK");
            }
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            //NavigationPage signUpPage = new NavigationPage(new SignupPage());
            //NavigationPage.SetHasNavigationBar(signUpPage, false);
            //Navigation.PushAsync(signUpPage);
            Application.Current.MainPage = new NavigationPage( new SignupPage());
        }
    }
}