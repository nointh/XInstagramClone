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

        public AddPostPage()
        {
            InitializeComponent();
            items = new ObservableCollection<Media>();
            BindingContext = this;
        }

        private async void btnPickMedia_Tapped(object sender, EventArgs e)
        {
            items.Clear();

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

                //file = video
                if (type.ToString().ToLower() == "video")
                {
                    var filePath = "";

                    var newFile = Path.Combine(FileSystem.AppDataDirectory, fileName + "." + extension);
                    using (var stream = await file.OpenReadAsync())
                    using (var newStream = File.OpenWrite(newFile))
                        await stream.CopyToAsync(newStream);
                    filePath = newFile;

                    items.Add(
                        new Media
                        {
                            VideoSource = MediaSource.FromFile(filePath),
                            Type = type.ToString().ToLower()
                        }
                    );
                }
                //file = image
                else
                {
                    using (var stream = await file.OpenReadAsync())
                    {
                        byte[] buffer;
                        long length = stream.Length;
                        buffer = new byte[length];
                        stream.Read(buffer, 0, (int)length);

                        items.Add(
                            new Media
                            {
                                Url = ImageSource.FromStream(() => new MemoryStream(buffer)),
                                Type = type.ToString().ToLower()
                            }
                        );
                    }
                }

                file.Dispose();
            }
        }

        private void btnProceedCaption_Tapped(object sender, EventArgs e)
        {
            if (items.Count > 0)
            {
                Navigation.PushAsync(new WriteCaptionPage(items[0], items.Count));
            }
        }
    }

    //public class PickMediaItem : INotifyPropertyChanged
    //{
    //    private string _Type;

    //    private ImageSource _Url;

    //    private MediaSource _VideoSource;

    //    public MediaSource VideoSource
    //    {
    //        get { return _VideoSource; }
    //        set
    //        {
    //            if (value != _VideoSource)
    //            {
    //                _VideoSource = value;
    //                OnPropertyChanged("VideoSource");
    //            }
    //        }
    //    }

    //    public ImageSource Url
    //    {
    //        get { return _Url; }
    //        set
    //        {
    //            if (value != _Url)
    //            {
    //                _Url = value;
    //                OnPropertyChanged("Url");
    //            }
    //        }
    //    }

    //    public string Type
    //    {
    //        get { return _Type; }
    //        set
    //        {
    //            if (value != _Type)
    //            {
    //                _Type = value;
    //                OnPropertyChanged("Type");
    //            }
    //        }
    //    }

    //    public event PropertyChangedEventHandler PropertyChanged;

    //    protected void OnPropertyChanged(string propertyName)
    //    {
    //        var handler = PropertyChanged;
    //        if (handler != null)
    //            handler(this, new PropertyChangedEventArgs(propertyName));
    //    }
    //}
}