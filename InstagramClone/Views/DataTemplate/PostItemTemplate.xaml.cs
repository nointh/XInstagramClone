using InstagramClone.Models;
using InstagramClone.Views.HomeTabbedPageViews;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InstagramClone.Views.DataTemplate
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PostItemTemplate : ContentView
    {
        PostModel currentPost; 
        public PostItemTemplate()
        {
            InitializeComponent();
        }
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            if (BindingContext != null)
                try
                {
                    currentPost = (PostModel)BindingContext;
                    Task.Run(async () =>
                    {
                        if (await FirebaseDB.IsPostSaved(currentPost))
                        {
                            SaveLabel.FontFamily = "FFASolid";
                        }
                    });
                }
                catch { }
        }
        private void Comment_Tapped(object sender, EventArgs e)
        {
            if (currentPost != null)
                Navigation.PushAsync(new CommentPage(currentPost.OwnerId, currentPost.PostId));
        }

        private void UnlikeEvent_Tapped(object sender, EventArgs e)
        {
            if (currentPost == null) return;

            currentPost.IsLiked = false;
            Task.Run(async () => await FirebaseDB.SetUnlikedToPost(currentPost));
            currentPost.LikedUsers = currentPost.LikedUsers.Where(u => u.UserId != FirebaseDB.CurrentUserId).ToList();
        }

        private void LikeEvent_Tapped(object sender, EventArgs e)
        {
            if (currentPost == null) return;

            currentPost.IsLiked = true;
            Task.Run(async () => await FirebaseDB.SetLikedToPost(currentPost));
            var tempList = currentPost.LikedUsers;
            tempList.Add(new UserLiked { UserId = FirebaseDB.CurrentUserId });
            currentPost.LikedUsers = tempList;
        }

        private async void userAvatar_Tapped(object sender, EventArgs e)
        {
            if (currentPost == null) return;

            FirebaseDB firebase = new FirebaseDB();
            if (currentPost.OwnerId == FirebaseDB.CurrentUserId)
            {
                await Navigation.PushAsync(new YourProfile(await FirebaseDB.GetCurentUserInfo()));
            }
            else
                await Navigation.PushAsync(new ProfilePage(await firebase.getUser(currentPost.OwnerId), await FirebaseDB.GetCurentUserInfo()));

        }

        private async void SavePost_Tapped(object sender, EventArgs e)
        {
            if (currentPost == null) return;

            
            if (await FirebaseDB.IsPostSaved(currentPost))
            {                
                SaveLabel.FontFamily = "FFARegular";
                Task.Run(async () => await FirebaseDB.UnsavePost(currentPost));
            }
            else
            {
                Task.Run(async () => await FirebaseDB.SavePost(currentPost));
                SaveLabel.FontFamily = "FFASolid";
            }
            
        }
    }
}