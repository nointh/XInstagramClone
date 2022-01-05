using InstagramClone.Models;
using InstagramClone.Views.HomeTabbedPageViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InstagramClone.Views.PostPageViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PostDetailPage : ContentPage
    {
        PostModel post = null;
        string postId = "";
        public PostDetailPage()
        {
            InitializeComponent();
        }
        public PostDetailPage(string postId)
        {
            InitializeComponent();
            this.postId = postId;
            InitPostData(postId);

        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            InitPostData(postId);
        }
        public async void InitPostData(string postId)
        {
            post = (await FirebaseDB.GetAllPostOfUser(FirebaseDB.CurrentUserId)).Where(i => i.PostId == postId).FirstOrDefault();
            BindPostData();
        }
        public void BindPostData()
        {
            if (post != null)
            {
                try
                {
                    TxtOwnerName.Text = post?.OwnerUsername;
                    TxtLowerUsername.Text = post?.OwnerUsername;
                    ImgAvatar.Source = post?.OwnerImage;
                    TxtLikeCount.Text = Convert.ToString(post.LikeCount);
                    TxtCaption.Text = post.Caption == null ? "" : post.Caption;
                    TxtPostTime.Text = post.PostTime == null ? "" : post.PostTime;
                    MediaList.ItemsSource = post?.MediaList;
                    Liked.IsVisible = post.IsLiked;
                    Unliked.IsVisible = !post.IsLiked;
                }
                catch { }
            }
        }
        private void userAvatar_Tapped(object sender, EventArgs e)
        {

        }

        private void UnlikeTap_Tapped(object sender, EventArgs e)
        {
            //ToggleHeartLabel(LikeLabel);
            post.IsLiked = false;
            Task.Run(async () => await FirebaseDB.SetUnlikedToPost(post));
            post.LikedUsers = post.LikedUsers.Where(u => u.UserId != FirebaseDB.CurrentUserId).ToList();
            Liked.IsVisible = post.IsLiked;
            Unliked.IsVisible = !post.IsLiked;
            TxtLikeCount.Text = Convert.ToString(post.LikeCount);
        }

        private void LikeTap_Tapped(object sender, EventArgs e)
        {
            post.IsLiked = true;
            Task.Run(async () => await FirebaseDB.SetLikedToPost(post));
            var tempList = post.LikedUsers;
            tempList.Add(new UserLiked { UserId = FirebaseDB.CurrentUserId });
            post.LikedUsers = tempList;
            Liked.IsVisible = post.IsLiked;
            Unliked.IsVisible = !post.IsLiked;
            TxtLikeCount.Text = Convert.ToString(post.LikeCount);
        }

        private async void lbComment_Tapped(object sender, EventArgs e)
        {
            if (post?.OwnerId != null && post?.PostId != null)
                await Navigation.PushAsync(new CommentPage(post.OwnerId, post.PostId));

        }
    }
}