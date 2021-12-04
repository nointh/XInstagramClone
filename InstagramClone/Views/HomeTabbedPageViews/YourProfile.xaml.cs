using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InstagramClone.Models;
using InstagramClone.Views.ProfilePageViews;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InstagramClone.Views.HomeTabbedPageViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class YourProfile : ContentPage
    {
        private UserModel user;
        public YourProfile(UserModel user)
        {
            this.user = user;
            InitializeComponent();
            InitProfile(this.user);
            InitStory(this.user);
            //InitImage(user);
        }

        private void InitProfile(UserModel user)
        {
            ProfileDescription.Text = user.ProfileDescription;
            UserFollowing.Text = "128";
            UserFollower.Text = "48";
            UserImg.Source = user.ImageUri;
        }
        private void InitStory(UserModel user)
        {
            List<StoryCollectionModel> stories = new List<StoryCollectionModel>
            {
                new StoryCollectionModel { CoverImage = "https://cdn.pixabay.com/photo/2021/10/19/10/56/cat-6723256_960_720.jpg", Title = "ABC" },
                new StoryCollectionModel { CoverImage = "https://cdn.pixabay.com/photo/2021/10/19/10/56/cat-6723256_960_720.jpg", Title = "ABC" },
                new StoryCollectionModel { CoverImage = "https://cdn.pixabay.com/photo/2021/10/19/10/56/cat-6723256_960_720.jpg", Title = "ABC" },
                new StoryCollectionModel { CoverImage = "https://cdn.pixabay.com/photo/2021/10/19/10/56/cat-6723256_960_720.jpg", Title = "ABC" },
                new StoryCollectionModel { CoverImage = "https://cdn.pixabay.com/photo/2021/10/19/10/56/cat-6723256_960_720.jpg", Title = "ABC" },
            };
            CollViewStory.ItemsSource = stories;
        }
        private void Button_Clicked(object sender, EventArgs e)
        {
            
        }
        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Image img = (Image)sender;
            DisplayAlert("Alert", "You click: " + img.Source, "OK");
        }

        private void EditProfile_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new NavigationPage(new EditProfilePage(this.user)));
        }

        private void options_Clicked(object sender, EventArgs e)
        {
            DisplayActionSheet("ActionSheet: Send to?", "Cancel", null, "Email", "Twitter", "Facebook");
        }
        private void viewPost_Clicked(object sender, EventArgs e)
        {
            Label lb = (Label)sender;
            DisplayAlert("View Post", lb.Text, "OK");
        }
        private void viewFollow_Clicked(object sender, EventArgs e)
        {
            Label lb = (Label)sender;
            Navigation.PushAsync(new NavigationPage(new FollowView()));
        }
    }
}