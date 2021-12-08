using System;
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
        private bool IsLoading = true;
        public AsyncCommand LoadNewsfeedCmd { get; }

        public HomePage()
        {
            InitializeComponent();
            LogoImage.Source = ImageSource.FromResource("InstagramClone.Resources.Images.InstagramLogo.svg.png");
            //InitData();
            LoadNewsfeedCmd = new AsyncCommand(LoadNewsfeed);
            ListViewPost.RefreshCommand = LoadNewsfeedCmd;
            
        }
        protected override void OnAppearing()
        {
            
            
            base.OnAppearing();
            LoadingIndicator.IsVisible = true;
            LoadingIndicator.IsRunning = true;

            Task.Run(LoadNewsfeed);
            
        }
        public async Task LoadNewsfeed()
        {
            LoadingIndicator.IsVisible = false;
            LoadingIndicator.IsRunning = false;
            var listPost = await FirebaseDB.GetNewsfeedPost();
            ListViewPost.ItemsSource = listPost;
            ListViewPost.IsRefreshing = false;
            LoadingIndicator.IsVisible = false;
            LoadingIndicator.IsRunning = false;
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
            ListViewPost.ItemsSource = PostModel.GetExamplePostList();
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Label LikeLabel = (Label)sender;
            ToggleHeartLabel(LikeLabel);
            var item = (PostModel)((Label)sender).BindingContext;
            DisplayAlert("Nội dung", "ID : " + item.Caption + "--" + FirebaseDB.CurrentUserId, "OK");

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

        private void lbComment_Tapped(object sender, EventArgs e)
        {
            var item = (PostModel)((Label)sender).BindingContext;
            Navigation.PushAsync(new CommentPage(item.OwnerId, item.PostId));
        }
    }
}