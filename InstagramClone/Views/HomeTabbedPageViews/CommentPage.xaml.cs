using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InstagramClone.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InstagramClone.Views.HomeTabbedPageViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CommentPage : ContentPage
    {
        List<FullCommentModel> cmts = new List<FullCommentModel>();
        SearchUserModel usr = new SearchUserModel();
        string selectedCmtId = "";
        string PostId = "";
        string OwnerId = "";

        public CommentPage()
        {
            InitializeComponent();
        }

        public CommentPage(string ownerid, string postid)
        {
            InitializeComponent();
            PostId = postid;
            OwnerId = ownerid;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            initData();
        }

        private async void initData()
        {
            cmts = new List<FullCommentModel>();
            var data = await FirebaseDB.GetPostComments(OwnerId, PostId);
            for (int i = 0; i < data.Count; i++)
            {
                FullCommentModel full = new FullCommentModel(data[i]);
                var likes = await FirebaseDB.GetCommentUserLiked(OwnerId, PostId, data[i].CommentId);
                //var usr = await FirebaseDB.GetUserById(data[i].OwnerId);

                //full.UserImage = usr.ImageUri;
                //full.Username = usr.Username;
                full.UserLiked = likes;
                full.LikeCount = full.UserLiked.Count;
                //imgUserAvatar.Source = usr.ImageUri;

                foreach (UserLiked like in likes)
                {
                    if (like.UserId == FirebaseDB.CurrentUserId)
                    {
                        full.IsLike = "Red";
                        break;
                    }
                }
                if (full.IsLike != "Red")
                    full.IsLike = "LightGray";

                cmts.Add(full);
            }
            ListViewComments.ItemsSource = cmts;

            usr = await FirebaseDB.GetUserById(FirebaseDB.CurrentUserId);
            imgUserAvatar.Source = usr.ImageUri;
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private async void btnLike_Tapped(object sender, EventArgs e)
        {
            Label heart = (Label)sender;
            if (heart.TextColor == Color.Red)
            {
                heart.TextColor = Color.LightGray;
                var item = (FullCommentModel)((Label)sender).BindingContext;
                await FirebaseDB.DeleteUserLikeForComment(OwnerId, PostId, item.CommentId, FirebaseDB.CurrentUserId);

                cmts[selectedHeartIndex(sender)].LikeCount--;
            }
            else
            {
                heart.TextColor = Color.Red;
                var item = (FullCommentModel)((Label)sender).BindingContext;
                await FirebaseDB.AddUserLikeForComment(OwnerId, PostId, item.CommentId, new UserLiked { UserId = FirebaseDB.CurrentUserId });

                cmts[selectedHeartIndex(sender)].LikeCount++;
            }
        }

        private int selectedHeartIndex(object sender)
        {
            var item = (FullCommentModel)((Label)sender).BindingContext;
            for (int i = 0; i < cmts.Count; i++)
            {
                if (cmts[i].Username == item.Username)
                    return i;
            }

            return -1;
        }

        private async void lbPostCmt_Tapped(object sender, EventArgs e)
        {
            if (editorCmt.Text.Length > 0)
            {
                CommentModel cmt = new CommentModel
                {
                    CommentDetail = editorCmt.Text,
                    Username = usr.Username,
                    UserImage = usr.ImageUri,
                    PostTime = DateTime.UtcNow.Date.ToString("dd/MM/yyyy")
                };

                await FirebaseDB.AddComment(OwnerId, PostId, cmt);

                editorCmt.Text = "";
                editorCmt.Unfocus();

                initData();
            }
        }

        private void ListViewComments_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var cmt = (FullCommentModel)ListViewComments.SelectedItem;
            selectedCmtId = cmt.CommentId;

            if (cmt.OwnerId == FirebaseDB.CurrentUserId)
            {
                gridCmt.IsVisible = false;
                gridDelete.IsVisible = true;
            }
            else
            {
                gridCmt.IsVisible = true;
                gridDelete.IsVisible = false;
            }
        }

        private void btnCancelDeleting_Clicked(object sender, EventArgs e)
        {
            gridCmt.IsVisible = true;
            gridDelete.IsVisible = false;

            ListViewComments.SelectedItem = null;
        }

        private async void btnDeleteComment_Clicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Delete comment?", "This action cannot be undo. Are you sure?", "Delete", "Cancel");
            if (answer)
            {
                await FirebaseDB.DeleteComment(OwnerId, PostId, selectedCmtId);
                btnCancelDeleting_Clicked(sender, e);
                initData();
            }
        }
    }
}