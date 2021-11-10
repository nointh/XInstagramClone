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
        private int PlusEditorHeight = 1;

        public CommentPage()
        {
            InitializeComponent();
            ListViewComments.ItemsSource = CommentModel.GetExampleCommentList();
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            DisplayAlert("Back button", "Tapped", "OK");
        }

        private void btnLike_Tapped(object sender, EventArgs e)
        {
            Label heart = (Label)sender;
            heart.FontFamily = "FFASolid";
            if (heart.TextColor == Color.Red)
            {
                heart.TextColor = Color.DimGray;
            }
            else heart.TextColor = Color.Red;
        }
    }
}