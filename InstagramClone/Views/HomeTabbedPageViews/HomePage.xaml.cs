using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using InstagramClone.Models;
using System.Windows.Input;

namespace InstagramClone.Views.HomeTabbedPageViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();
            LogoImage.Source = ImageSource.FromResource("InstagramClone.Resources.Images.InstagramLogo.svg.png");
            InitData();
            
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            var listPost = await FirebaseDB.GetAllPostOfUser(FirebaseDB.CurrentUserId);
            foreach(var item in listPost)
            {
                Console.WriteLine(item.PostId);
                Console.WriteLine(item.Caption);
                var x = await FirebaseDB.GetMediaListOfPost(FirebaseDB.CurrentUserId, item.PostId);
            }
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

            CollViewStory.ItemsSource = list;
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
    }
}