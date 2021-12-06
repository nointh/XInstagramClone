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
        private List<FollowUser> Follower;
        private List<FollowUser> Following;

        public YourProfile(UserModel user)
        {
            this.user = user;
            InitializeComponent();
            InitProfile(this.user);
            //InitStory(this.user);
            //InitImage(user);
        }

        private void InitProfile(UserModel user)
        {
            Title = user.Username;
            ProfileDescription.Text = user.ProfileDescription;
            if (Following != null && Follower != null)
            {
                UserFollowing.Text = Following.Count.ToString();
                UserFollower.Text = Follower.Count.ToString();
            }
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
            Title = user.Username;
            Navigation.PushAsync(new NavigationPage(new EditProfilePage(this.user)));
        }
        private void viewPost_Clicked(object sender, EventArgs e)
        {
            Label lb = (Label)sender;
            DisplayAlert("View Post", lb.Text, "OK");
        }
        private void viewFollow_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new NavigationPage(new FollowView(Follower, Following)));
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            FirebaseDB fb = new FirebaseDB();
            user = await fb.getUser(user.Username);
            Following = await fb.getFollowing(user.Username);
            Follower = await fb.getFollower(user.Username);
            InitProfile(user);
        }
    }
}