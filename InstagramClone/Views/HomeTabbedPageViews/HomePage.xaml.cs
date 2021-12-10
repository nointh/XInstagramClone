﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using InstagramClone.Models;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;

namespace InstagramClone.Views.HomeTabbedPageViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        //private bool IsLoading = true;
        public AsyncCommand RefreshCommand;
        public HomePage()
        {
            InitializeComponent();
            LogoImage.Source = ImageSource.FromResource("InstagramClone.Resources.Images.InstagramLogo.svg.png");
            //InitData();
            RefreshCommand = new AsyncCommand(LoadNewsfeedItemsAsync);
            PostRefresh.Command = RefreshCommand;

        }
        protected override void OnAppearing()
        {
            
            
            base.OnAppearing();
            PostRefresh.IsRefreshing = true;
            Task.Run(LoadNewsfeedItemsAsync);

        }
        //public void ToggleLoadingIndicator()
        //{
        //    if (LoadingIndicator.IsRunning)
        //    {
        //        LoadingIndicator.IsVisible = false;
        //        LoadingIndicator.IsRunning = false;
        //        LoadingIndicator.IsEnabled = false;
        //        return;
        //    }
        //    LoadingIndicator.IsVisible = true;
        //    LoadingIndicator.IsRunning = true;
        //    LoadingIndicator.IsEnabled = true;

        //}
        public void LoadNewsfeed()
        {
            //PostRefresh.IsRefreshing = true;
            Task.Run(LoadNewsfeedItemsAsync);

        }
        public async Task LoadNewsfeedItemsAsync()
        {
            var listPost = await FirebaseDB.GetNewsfeedPost();
            CollectionViewPost.ItemsSource = listPost;
            PostRefresh.IsRefreshing = false;
        }
        public void InitData()
        {
            List<UserModel> list = new List<UserModel>
            {
                new UserModel { Username = "ntncxzcaaaaaaaaaaaaaaaaaaaaaaaa", Fullname = "Nguyen Thanh Noi", ImageUri = "https://randomuser.me/api/portraits/men/51.jpg" },
                new UserModel { Username = "Shizune", Fullname = "Nguyen Thanh Noi", ImageUri = "https://randomuser.me/api/portraits/women/51.jpg" },
                new UserModel { Username = "Ngthclone", Fullname = "Nguyen Thanh Noi", ImageUri = "https://randomuser.me/api/portraits/men/26.jpg" },
                new UserModel { Username = "Andre", Fullname = "Nguyen Thanh Noi", ImageUri = "https://randomuser.me/api/portraits/women/64.jpg" },
                new UserModel { Username = "Taka", Fullname = "Nguyen Thanh Noi", ImageUri = "https://randomuser.me/api/portraits/men/72.jpg" }
            };

            //CollViewStory.ItemsSource = list;
            CollectionViewPost.ItemsSource = PostModel.GetExamplePostList();
        }

        //set unlike post tap
        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Label LikeLabel = (Label)sender;
            //ToggleHeartLabel(LikeLabel);
            var item = (PostModel)((Label)sender).BindingContext;
            item.IsLiked = false;
            Task.Run(async () => await FirebaseDB.SetUnlikedToPost(item));
            item.LikedUsers = item.LikedUsers.Where(u => u.UserId != FirebaseDB.CurrentUserId).ToList();
        }

        private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            DisplayAlert("Title", "You have opened the send", "OK");
        }

        private void btnAddPost_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddPostPage());
        }
        
        public void ToggleHeartLabel(Label LikeLabel)
        {
            if (LikeLabel.FontFamily == "FFARegular")
            {
                LikeLabel.FontFamily = "FFASolid";
                LikeLabel.TextColor = Color.Red;
            }
            else
            {
                LikeLabel.FontFamily = "FFARegular";
                LikeLabel.TextColor = Color.Black;
            }
        }

        private void TapGestureRecognizer_Tapped_2(object sender, EventArgs e)
        {
            CarouselView carouselView = (CarouselView)sender;
            Grid gridLayout = (Grid)carouselView.Parent;
            StackLayout subPost = (StackLayout)gridLayout.Children.Where(c => Grid.GetRow(c) == 2);
            Label heartLabel = (Label)subPost.Children[0];
            heartLabel.FontFamily = "FFASolid";
            heartLabel.TextColor = Color.Red;
        }

        private async void lbComment_Tapped(object sender, EventArgs e)
        {
            var item = (PostModel)((Label)sender).BindingContext;
            await Navigation.PushAsync(new CommentPage(item.OwnerId, item.PostId));
        }
        //set like post tapp
        private void TapGestureRecognizer_Tapped_3(object sender, EventArgs e)
        {
            var item = (PostModel)((Label)sender).BindingContext;
            item.IsLiked = true;
            Task.Run(async () => await FirebaseDB.SetLikedToPost(item));
            var tempList = item.LikedUsers;
            tempList.Add(new UserLiked { UserId = FirebaseDB.CurrentUserId });
            item.LikedUsers = tempList;
        }
    }
}