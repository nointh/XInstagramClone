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

        private void editorCmt_TextChanged(object sender, TextChangedEventArgs e)
        {
            var CharacterWidth = editorCmt.FontSize;
            var CharacterCount = editorCmt.Text.Length;

            lbPostCmt.IsEnabled = CharacterCount > 0 ? true : false;
            lbPostCmt.Opacity = CharacterCount > 0 ? 1 : 0.5;

            if (CharacterCount <= 70)
            {
                if ((CharacterCount * CharacterWidth) > (editorCmt.Width * PlusEditorHeight) + (CharacterWidth * 20))
                {
                    Console.WriteLine(CharacterCount);
                    gridCmt.HeightRequest += CharacterWidth;
                    editorCmt.HeightRequest = gridCmt.HeightRequest;
                    PlusEditorHeight++;
                }
                else
                {
                    if ((PlusEditorHeight > 1) &&
                        (CharacterCount * CharacterWidth) < (editorCmt.Width * (PlusEditorHeight - 1)) + (CharacterWidth * 20))
                    {
                        gridCmt.HeightRequest -= CharacterWidth;
                        editorCmt.HeightRequest = gridCmt.HeightRequest;
                        PlusEditorHeight--;
                    }
                }
            }
            else
            {
                var val = e.NewTextValue.Substring(0, 70);
                editorCmt.Text = val;
            }
        }
    }
}