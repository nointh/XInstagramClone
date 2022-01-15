using InstagramClone.Models;
using NativeMedia;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InstagramClone.Views.ProfilePageViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChangeAvatarPage : ContentPage
    {
        public UserModel currentUser;
        public List<Stream> pickFile = new List<Stream>();
        public string fName = "";
        private string newPhotoUrl = "";

        public ChangeAvatarPage()
        {
            InitializeComponent();
        }

        public ChangeAvatarPage(UserModel usr)
        {
            InitializeComponent();
            currentUser = usr;
            UserImg.Source = usr.ImageUri;
        }

        private async void btnChoosePhoto_Clicked(object sender, EventArgs e)
        {
            pickFile.Clear();

            var cancelToken = new CancellationTokenSource();
            IMediaFile[] files = null;

            try
            {
                var request = new MediaPickRequest(1, MediaFileType.Image)
                {
                    PresentationSourceBounds = System.Drawing.Rectangle.Empty,
                    UseCreateChooser = true,
                    Title = "Select your photo"
                };

                cancelToken.CancelAfter(TimeSpan.FromMinutes(1));

                var results = await MediaGallery.PickAsync(request, cancelToken.Token);

                files = results?.Files?.ToArray();
            }
            catch (OperationCanceledException)
            {

            }
            catch (Exception)
            {

            }
            finally
            {
                cancelToken.Dispose();
            }

            if (files == null)
            {
                return;
            }

            foreach (var file in files)
            {
                var fileName = file.NameWithoutExtension;
                var extension = file.Extension;

                byte[] buffer;
                using (var stream = await file.OpenReadAsync())
                {
                    long length = stream.Length;
                    buffer = new byte[length];
                    stream.Read(buffer, 0, (int)length);
                }

                fName = fileName + "." + extension;

                pickFile.Add(new MemoryStream(buffer));
                UserImg.Source = ImageSource.FromStream(() => new MemoryStream(buffer));

                btnSave.IsVisible = true;

                file.Dispose();
            }
        }

        private async void btnSave_Clicked(object sender, EventArgs e)
        {
            FirebaseDB db = new FirebaseDB();
            btnSave.IsVisible = false;
            btnChoosePhoto.IsVisible = false;
            activityIndicator.IsVisible = true;

            await UploadNewFile();
            currentUser.ImageUri = newPhotoUrl;
            await db.updateUser(currentUser);

            activityIndicator.IsVisible = false;
            await DisplayAlert("Success", "Your profile photo has been updated!", "OK");
            await Navigation.PopAsync();
        }

        private async Task UploadNewFile()
        {
            newPhotoUrl = await FirebaseDB.UploadFile(pickFile[0], fName);
        }
    }
}