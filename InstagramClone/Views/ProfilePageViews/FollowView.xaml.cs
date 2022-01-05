using System;
using System.Collections.Generic;
using System.Linq;
using Firebase.Database;
using InstagramClone.Models;
using InstagramClone.Views.HomeTabbedPageViews;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.CommunityToolkit.UI.Views;

namespace InstagramClone.Views.ProfilePageViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FollowView : ContentPage
    {
        FirebaseDB db = new FirebaseDB();
        UserModel user;
        string isViewing;
        List<UserModel> Follower = new List<UserModel>();
        List<UserModel> Following = new List<UserModel>();
        List<FollowUser> FollowerKey;
        List<FollowUser> FollowingKey;
        public FollowView(List<FollowUser> Follower, List<FollowUser> Following, UserModel user)
        {
            this.user = user;
            Title = this.user.Username;
            InitializeComponent();
            this.FollowerKey = Follower;
            this.FollowingKey = Following;
            isViewing = "follower";
            InitFollower();
            InitFollowing();
        }

        async private void InitFollower()
        {
            foreach (FollowUser user in FollowerKey)
            {
                Follower.Add(await db.getUserByKey(user.UserKey));
            }
            FollowerList.ItemsSource = Follower;
        }
        async private void InitFollowing()
        {
            FollowingList.ItemsSource = Following;
            foreach (FollowUser user in FollowingKey)
            {
                Following.Add(await db.getUserByKey(user.UserKey));
            }
        }
        async private void btnFollow_Clicked(object sender, EventArgs e)
        {
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
        private void TabView_SelectionChanged(object sender, TabSelectionChangedEventArgs e)
        {
            if (e.NewPosition == 0)
            {
                isViewing = "follower";
                //InitFollower();
            } else
            {
                isViewing = "following";
                //InitFollowing();
            }
        }
        private void FindUser_TextChanged(object sender, TextChangedEventArgs e)
        {
            string toFind = e.NewTextValue;
            if (isViewing == "follower")
            {
                List<UserModel> searchFollwerList = new List<UserModel>();
                foreach (UserModel user in Follower)
                {
                    if (user.Username.Contains(toFind))
                    {
                        searchFollwerList.Add(user);
                    }
                }
                FollowerList.ItemsSource = searchFollwerList;
            } 
            else
            {
                List<UserModel> searchFollwingList = new List<UserModel>();
                foreach (UserModel user in Following)
                {
                    if (user.Username.Contains(toFind))
                    {
                        searchFollwingList.Add(user);
                    }
                }
                FollowingList.ItemsSource = searchFollwingList;
            }
        }
        async private void Follow_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            UserModel u = (UserModel)e.SelectedItem;
            await Navigation.PushAsync(new ProfilePage(u, user));
        }
    }
}