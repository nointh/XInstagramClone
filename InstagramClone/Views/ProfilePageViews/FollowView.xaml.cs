using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InstagramClone.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InstagramClone.Views.ProfilePageViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FollowView : ContentPage
    {
        public FollowView()
        {
            InitializeComponent();
            InitFollow();
        }

        private void InitFollow()
        {
            List<StoryCollectionModel> stories = new List<StoryCollectionModel>
            {
                new StoryCollectionModel { CoverImage = "https://cdn.pixabay.com/photo/2021/10/19/10/56/cat-6723256_960_720.jpg", Title = "ABC" },
                new StoryCollectionModel { CoverImage = "https://cdn.pixabay.com/photo/2021/10/19/10/56/cat-6723256_960_720.jpg", Title = "ABC" },
                new StoryCollectionModel { CoverImage = "https://cdn.pixabay.com/photo/2021/10/19/10/56/cat-6723256_960_720.jpg", Title = "ABC" },
                new StoryCollectionModel { CoverImage = "https://cdn.pixabay.com/photo/2021/10/19/10/56/cat-6723256_960_720.jpg", Title = "ABC" },
                new StoryCollectionModel { CoverImage = "https://cdn.pixabay.com/photo/2021/10/19/10/56/cat-6723256_960_720.jpg", Title = "ABC" },
            };
            myLV.ItemsSource = stories;
        }
    }
}