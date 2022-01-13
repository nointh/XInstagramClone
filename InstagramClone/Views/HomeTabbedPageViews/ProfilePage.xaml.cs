﻿using InstagramClone.Views.LoginPageViews;
using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using Firebase.Database;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using InstagramClone.Models;
using InstagramClone.Views.ProfilePageViews;
using InstagramClone.Views.PostPageViews;


namespace InstagramClone.Views.HomeTabbedPageViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        FirebaseDB fb = new FirebaseDB();
        private UserModel user = new UserModel();
        private UserModel you = new UserModel();
        private List<FollowUser> Follower;
        private List<FollowUser> Following;
        private List<PostModel> Posts = new List<PostModel>();

        public ProfilePage(UserModel user, UserModel you)
        {
            this.user = user;
            this.you = you;
            InitializeComponent();
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            user = await fb.getUser(user.UID);
            Following = await fb.getFollowing(user.UID);
            Follower = await fb.getFollower(user.UID);
            Posts = await FirebaseDB.GetAllPostOfUser(user.UID);

            InitProfile();
            InitButton();
        }
        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Image img = (Image)sender;
            Navigation.PushAsync(new PostDetailPage(img.ClassId));
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
            if (Posts != null)
            {
                UserPost.Text = Posts.Count.ToString();
                InitPost();
            }
            if (user.ImageUri != null)
            {
                UserImg.Source = user.ImageUri;
            }
        }
        private void InitPost()
        {
            UserPosts.ItemsSource = Posts;
        }

        async private void InitButton()
        {
            Button btn = FollowButton;
            var result = await fb.checkIsFollow(you.UID, user.UID);
            if (result)
            {
                btn.Text = "Unfollow";
                btn.BackgroundColor = Color.White;
                btn.TextColor = Color.Black;
                btn.BorderColor = Color.Black;
            } 
            else
            {
                btn.Text = "Follow";
                btn.BackgroundColor = Color.FromHex("#405DE6");
                btn.BorderColor = Color.FromHex("#405DE6");
                btn.TextColor = Color.White;
            }
        }

        private void Button_Clicked(object sender, EventArgs e)
        {            
            
        }

        private void viewFollow_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FollowView(Follower, Following, user));
        }

        async private void FollowButton_Clicked(object sender, EventArgs e)
        {
            Button btn = FollowButton;
            try
            {
                FollowUser user1 = new FollowUser();
                user1.UserKey = you.UID;
                FollowUser user2 = new FollowUser();
                user2.UserKey = user.UID;
                if (await fb.updateFollow(user1, user2) == "unfollow")
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

        private void MessageButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new TestChatBox(user.UID));
        }
    }
}