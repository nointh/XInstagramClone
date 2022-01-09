using System;
using System.Collections.Generic;
using InstagramClone.Models;
using InstagramClone.Views.ProfilePageViews;
using InstagramClone.Views.LoginPageViews;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InstagramClone.Views.HomeTabbedPageViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class YourProfile : ContentPage
    {
        private UserModel user;
        private List<FollowUser> Follower;
        private List<FollowUser> Following;

        public YourProfile(UserModel user)
        {
            InitializeComponent();
            this.user = user;

        }
        private void InitProfile()
        {
            Title = user.Username;
            if (user.ProfileDescription != null)
            {
                ProfileDescription.Text = user.ProfileDescription;
            } 
            if (Following != null && Follower != null)
            {
                UserFollowing.Text = Following.Count.ToString();
                UserFollower.Text = Follower.Count.ToString();
            }
            if (user.ImageUri != null)
            {
                UserImg.Source = user.ImageUri;
            }
        }
        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Image img = (Image)sender;
            DisplayAlert("Alert", "You click: " + img.Source, "OK");
        }
        private void EditProfile_Clicked(object sender, EventArgs e)
        {
            Title = user.Username;
            Navigation.PushAsync(new EditProfilePage(this.user));
        }
        private void viewPost_Clicked(object sender, EventArgs e)
        {
            Label lb = (Label)sender;
            DisplayAlert("View Post", lb.Text, "OK");
        }
        private void viewFollow_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FollowView(Follower, Following, user));
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            FirebaseDB fb = new FirebaseDB();
            user = await fb.getUser(user.UID);
            Following = await fb.getFollowing(user.UID);
            Follower = await fb.getFollower(user.UID);
            InitProfile();
        }
        private void ViewMore_Clicked(object sender, EventArgs e)
        {
            CollViewStory.IsVisible = !CollViewStory.IsVisible;
        }
        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            Preferences.Set("FirebaseRefreshToken", null);
            NavigationPage loginPage = new NavigationPage(new LoginPage());
            NavigationPage.SetHasNavigationBar(loginPage, false);
            //Navigation.PushAsync(loginPage);
            Application.Current.MainPage = new NavigationPage(loginPage);
        }
    }
}