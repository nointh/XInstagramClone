using System;
using System.Collections.Generic;
using InstagramClone.Models;
using Firebase.Database;
using InstagramClone.Views.ProfilePageViews;
using InstagramClone.Views.LoginPageViews;
using InstagramClone.Views.PostPageViews;
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
        private List<PostModel> Posts;
        private List<UserModel> suggestFollow;

        public YourProfile(UserModel user)
        {
            InitializeComponent();
            this.user = user;

        }
        private void InitProfile()
        {
            TitleUsername.Text = user.Username;
            if (user.ProfileDescription != null)
            {
                ProfileDescription.Text = user.ProfileDescription;
            } 
            if (Following != null && Follower != null)
            {
                UserFollowing.Text = Following.Count.ToString();
                UserFollower.Text = Follower.Count.ToString();
            }
            if (Posts != null)
            {
                UserPost.Text = Posts.Count.ToString();
                InitPost();
            }
            if (user.ImageUri != null)
            {
                UserImg.Source = user.ImageUri;
            }
            InitSuggestFollow();
        }
        private void InitPost()
        {
            UserPosts.ItemsSource = Posts;
        }
        async private void InitSuggestFollow()
        {
            FirebaseDB db = new FirebaseDB();
            List<UserModel> users = await db.getSuggestFollow(user.UID, Following);
            CollSuggestFollow.ItemsSource = users;
        }
        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Image img = (Image)sender;
            Navigation.PushAsync(new PostDetailPage(img.ClassId));
        }
        private void Logout(object sender, EventArgs e)
        {
            Preferences.Set("FirebaseRefreshToken", null);
            NavigationPage loginPage = new NavigationPage(new LoginPage());
            NavigationPage.SetHasNavigationBar(loginPage, false);
            Navigation.PushAsync(loginPage);
        }
        private void EditProfile_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new EditProfilePage(this.user));
        }
        private void viewPost_Clicked(object sender, EventArgs e)
        {
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
            Posts = await FirebaseDB.GetAllPostOfUser(user.UID);

            InitProfile();
        }
        private void ViewMore_Clicked(object sender, EventArgs e)
        {
            SuggestFollow.IsVisible = !SuggestFollow.IsVisible;
        }
        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            Preferences.Set("FirebaseRefreshToken", null);
            NavigationPage loginPage = new NavigationPage(new LoginPage());
            NavigationPage.SetHasNavigationBar(loginPage, false);
            //Navigation.PushAsync(loginPage);
            Application.Current.MainPage = new NavigationPage(loginPage);
        }

        async private void FollowButton_Clicked(object sender, EventArgs e)
        {
            FirebaseDB db = new FirebaseDB();
            Button btn = (Button)sender;
            var UserKey = btn.ClassId;
            try
            {
                FollowUser user1 = new FollowUser();
                user1.UserKey = user.UID;
                FollowUser user2 = new FollowUser();
                user2.UserKey = UserKey;
                if (await db.updateFollow(user1, user2) == "unfollow")
                {
                    btn.Text = "Follow";
                    btn.BackgroundColor = Color.FromHex("#405DE6");
                    btn.BorderColor = Color.FromHex("#405DE6");
                    btn.TextColor = Color.White;
                }
                else
                {
                    btn.Text = "Unfollow";
                    btn.BackgroundColor = Color.White;
                    btn.TextColor = Color.Black;
                    btn.BorderColor = Color.Black;
                }

            }
            catch (FirebaseException ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            Image img = (Image)sender;
            UserModel temp = new UserModel();
            temp.UID = img.ClassId;
            Navigation.PushAsync(new ProfilePage(temp, user));
        }
    }
}