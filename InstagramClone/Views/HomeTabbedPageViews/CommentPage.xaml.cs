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
        FirebaseDB firebase = new FirebaseDB();
        List<FullCommentModel> cmts = new List<FullCommentModel>();
        string selectedCmtId = "";

        public CommentPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            initData();
        }

        private async void initData()
        {
            cmts = new List<FullCommentModel>();
            var data = await firebase.GetPostComments("postid");
            for (int i = 0; i < data.Count; i++)
            {
                FullCommentModel full = new FullCommentModel(data[i]);
                var likes = await firebase.GetCommentUserLiked("postid", data[i].CommentId);
                full.UserLiked = likes;
                full.LikeCount = full.UserLiked.Count;
                cmts.Add(full);
            }
            ListViewComments.ItemsSource = cmts;
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            DisplayAlert("Back button", "Tapped", "OK");
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
                await firebase.DeleteUserLikeForComment("postid", item.CommentId, "zzz");

                cmts[selectedHeartIndex(sender)].LikeCount--;
            }
            else
            {
                heart.TextColor = Color.Red;
                var item = (FullCommentModel)((Label)sender).BindingContext;
                await firebase.AddUserLikeForComment("postid", item.CommentId, new UserLiked { Username = "zzz" });

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

                await firebase.AddComment("postid", cmt);

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
                await firebase.DeleteComment("postid", selectedCmtId);
                btnCancelDeleting_Clicked(sender, e);
                initData();
            }
        }
    }
}