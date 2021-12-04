using InstagramClone.Views.LoginPageViews;
using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using InstagramClone.Models;

namespace InstagramClone.Views.HomeTabbedPageViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage(UserModel user)
        {
            InitializeComponent();
            InitProfile(user);
            InitStory(user);
            InitImage(user);
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
        private void InitImage(UserModel user)
        {
            List<string> img = new List<string>
            {
                "https://cdn.pixabay.com/photo/2021/10/19/10/56/cat-6723256_960_720.jpg",
                "https://cdn.pixabay.com/photo/2021/10/19/10/56/cat-6723256_960_720.jpg",
                "https://cdn.pixabay.com/photo/2021/10/19/10/56/cat-6723256_960_720.jpg",
            }; 

            //profileGrid.HeightRequest = 100 * (img.Count / 3 + 1);
            var imageGrid = new Grid();
            int r =  img.Count / 3 + 1;
            for (int i = 0; i < r; i++)
            {
                imageGrid.RowDefinitions.Add(new RowDefinition { Height = 100 });
            }
            for (int j = 0; j < 3; j++)
            {
                imageGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }
            int row = 0;
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) => {
                TapGestureRecognizer_Tapped(s, e);
            };
            
            //image.GestureRecognizers.Add(tapGestureRecognizer);
            for (int num = 0; num < img.Count; num++)
            {
                var image = new Image();
                image.HeightRequest = 100;
                if (num % 3 == 0)
                {
                    image.Source = img[num];
                    imageGrid.Children.Add(image, 0, row);

                }
                else if (num % 3 == 1)
                {
                    image.Source = img[num];
                    imageGrid.Children.Add(image, 1, row);
                }
                else 
                {
                    image.Source = img[num];
                    imageGrid.Children.Add(image, 2, row);
                }
                row++;
            }
            ImageGrid = imageGrid;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {            
            Preferences.Set("FirebaseRefreshToken", null);
            NavigationPage loginPage = new NavigationPage(new LoginPage());
            NavigationPage.SetHasNavigationBar(loginPage, false);
            Navigation.PushAsync(loginPage);
        }

        private void FollowButton_Clicked(object sender, EventArgs e)
        {

        }

        private void MessageButton_Clicked(object sender, EventArgs e)
        {

        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Image img = (Image)sender;
            DisplayAlert("Alert", "You click: " + img.Source, "OK");
        }
    }
}