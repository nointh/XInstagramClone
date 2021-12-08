using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;
using System.Collections.ObjectModel;
using NativeMedia;
using Xamarin.CommunityToolkit.Core;
using Xamarin.Essentials;
using System.Threading;
using InstagramClone.Models;

namespace InstagramClone.Views.HomeTabbedPageViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddPostPage : ContentPage
    {
        public ObservableCollection<Media> items { get; set; }
        public List<Stream> PickFiles = new List<Stream>();
        public List<string> fName = new List<string>();

        public AddPostPage()
        {
            InitializeComponent();
            items = new ObservableCollection<Media>();
            BindingContext = this;
        }

        private async void btnPickMedia_Tapped(object sender, EventArgs e)
        {
            items.Clear();
            PickFiles.Clear();
            fName.Clear();

            var cancelToken = new CancellationTokenSource();
            IMediaFile[] files = null;

            try
            {
                var request = new MediaPickRequest(5, MediaFileType.Image, MediaFileType.Video)
                {
                    PresentationSourceBounds = System.Drawing.Rectangle.Empty,
                    UseCreateChooser = true,
                    Title = "Select up to 5 items"
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
                var contentType = file.ContentType;
                var type = file.Type;
                var newFile = Path.Combine(FileSystem.AppDataDirectory, fileName + "." + extension);

                //file = video
                if (type.ToString().ToLower() == "video")
                {
                    var filePath = "";

                    using (var stream = await file.OpenReadAsync())
                    using (var newStream = File.OpenWrite(newFile))
                    {
                        await stream.CopyToAsync(newStream);
                    }
                    
                    filePath = newFile;

                    items.Add(
                        new Media
                        {
                            VideoSource = MediaSource.FromFile(filePath),
                            Type = type.ToString().ToLower()
                        }
                    );
                }

                FileStream fileStream = new FileStream(newFile, FileMode.Open, FileAccess.Read);

                byte[] buffer;
                long length = fileStream.Length;
                buffer = new byte[length];
                fileStream.Read(buffer, 0, (int)length);

                fName.Add(fileName + "." + extension);

                PickFiles.Add(new MemoryStream(buffer));

                if (type.ToString().ToLower() != "video")
                {
                    items.Add(
                        new Media
                        {
                            Url = ImageSource.FromStream(() => new MemoryStream(buffer)),
                            Type = type.ToString().ToLower()
                        }
                    );
                }

                fileStream.Close();

                file.Dispose();
            }
        }

        private void btnProceedCaption_Tapped(object sender, EventArgs e)
        {
            if (items.Count > 0)
            {
                Navigation.PushAsync(new WriteCaptionPage(items, PickFiles, fName));
            }
        }
    }
}