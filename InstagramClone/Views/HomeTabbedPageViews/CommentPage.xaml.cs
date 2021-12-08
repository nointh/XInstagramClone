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
                full.UserLiked = likes;
                full.LikeCount = full.UserLiked.Count;
                cmts.Add(full);
                Console.WriteLine(full.PostTime);
            }
            ListViewComments.ItemsSource = cmts;
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private async void btnLike_Tapped(object sender, EventArgs e)
        {
            Label heart = (Label)sender;
            heart.FontFamily = "FFASolid";
            if (heart.TextColor == Color.Red)
            {
                heart.FontFamily = "FFARegular";
                heart.TextColor = Color.DimGray;
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
                CommentModel cmt = new CommentModel();
                cmt.CommentDetail = editorCmt.Text;
                cmt.Username = "dungtd";
                cmt.UserImage = "https://randomuser.me/api/portraits/men/52.jpg";
                cmt.PostTime = "6/12/2021";

                await FirebaseDB.AddComment(OwnerId, PostId, cmt);

                editorCmt.Text = "";
                editorCmt.Unfocus();

                initData();
            }
        }

        private void ListViewComments_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            gridCmt.IsVisible = false;
            gridDelete.IsVisible = true;

            selectedCmtId = ((FullCommentModel)ListViewComments.SelectedItem).CommentId;
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