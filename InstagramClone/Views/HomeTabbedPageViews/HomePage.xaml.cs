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
            Console.WriteLine("label tapped");
            Label LikeLabel = (Label)sender;
            LikeLabel.FontFamily = "FFASolid";
            LikeLabel.TextColor = Color.Red;
        }

        private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            DisplayAlert("Title", "You have opened the send", "OK");
        }

        private void btnAddPost_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddPostPage());
        }
    }
}