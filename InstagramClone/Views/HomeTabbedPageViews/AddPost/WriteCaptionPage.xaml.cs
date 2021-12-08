using InstagramClone.Models;
using NativeMedia;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InstagramClone.Views.HomeTabbedPageViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WriteCaptionPage : ContentPage
    {
        FirebaseDB firebase = new FirebaseDB();
        ObservableCollection<Media> items = new ObservableCollection<Media>();
        List<Stream> pickFiles = new List<Stream>();
        List<string> fileName = new List<string>();
        List<string> Url = new List<string>();

        public WriteCaptionPage()
        {
            InitializeComponent();
        }

        public WriteCaptionPage(ObservableCollection<Media> picked, List<Stream> picks, List<string> fName)
        {
            InitializeComponent();
            items = picked;
            pickFiles = picks;
            fileName = fName;
            initViews();
        }

        private void btnBack_Tapped(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void initViews()
        {
            //Frame items quantity
            if (items.Count > 1)
            {
                lbQuantity.Text = "+" + (items.Count - 1).ToString();
            }
            else
            {
                frQuantity.IsVisible = false;
            }

            //Frame img/video
            if (items[0].Type == "video")
            {
                imgFirstItem.Source = "https://media.istockphoto.com/vectors/video-clip-player-icon-black-minimalist-icon-isolated-on-white-vector-id867290694?k=20&m=867290694&s=170667a&w=0&h=OYq8MB-wj9o1r2_w6-c8AKOlSf4S2PyE8Z3rzzbuWEk=";
            }
            else
            {
                imgFirstItem.Source = items[0].Url;
            }
        }

        private async void btnFinishPost_Tapped(object sender, EventArgs e)
        {
            setUpControlProcessing(true);

            await UploadMediaFile();

            PostModelBasic post = new PostModelBasic();
            post.OwnerId = "tngdcdng";
            post.PostTime = "7/12/2021";
            post.Caption = editorCaption.Text;

            await firebase.AddPost(post);

            string postId = await firebase.GetPostId(post);

            await AddMediaListToPost(postId);

            await DisplayAlert("Success", "Added post successfully!", "OK");
        }

        private void setUpControlProcessing(bool work)
        {
            if (work)
            {
                LoadingIndicator.IsVisible = true;
                LoadingIndicator.IsRunning = true;
                lbFinish.IsVisible = false;
                lbBack.IsEnabled = false;
                editorCaption.Unfocus();
            }
            else
            {
                LoadingIndicator.IsVisible = false;
                lbBack.IsEnabled = true;
            }
        }

        private async Task UploadMediaFile()
        {
            for (int i = 0; i < pickFiles.Count; i++)
            {
                Url.Add(await firebase.UploadFile(pickFiles[i], fileName[i]));
                Console.WriteLine(Url[i]);
            }
        }

        private async Task AddMediaListToPost(string postId)
        {
            for (int i = 0; i < items.Count; i++)
            {
                MediaContent media = new MediaContent(items[i].Type, Url[i]);
                await firebase.AddMediaToPost(postId, media);
            }
        }
    }
}