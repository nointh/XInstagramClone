using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using Xamarin.CommunityToolkit.Core;
using Xamarin.Forms;

namespace InstagramClone.Models
{
    class PostModel : INotifyPropertyChanged
    {
        public string PostId { get; set; }
        public string OwnerId { get; set; }
        public string OwnerUsername { get; set; }
        public string OwnerImage { get; set; }
        public string PostImage { get; set; }
        public ObservableCollection<Media> MediaList { get; set; }
        public string Caption { get; set; }
        public string PostTime { get; set; }
        private bool _isLiked { get; set; }
        public bool IsLiked { 
            get {
                return _isLiked;
            } 
            set {
                if (value != _isLiked)
                {
                    _isLiked = value;
                    OnPropertyChanged("IsLiked");
                }
            } }
        private int _likeCount;
        public int LikeCount
        {
            get
            {
                return _likeCount;
            }
            set
            {
                if (value != _likeCount)
                {
                    _likeCount = value;
                    OnPropertyChanged("LikeCount");
                }
            }
        }
        private List<UserLiked> _LikedUsers;
        public List<UserLiked> LikedUsers {
            get
            {
                return _LikedUsers;
            }
            set
            {
                _LikedUsers = value;
                LikeCount = value.Count;
                OnPropertyChanged("LikedUsers");
            }
        }
        public PostModel()
        {
            PostId = "";
            PostImage = "";
            IsLiked = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
        public static ObservableCollection<PostModel> GetExamplePostList()
        {
            return new ObservableCollection<PostModel>()
            {
                //new PostModel(){ PostId = "a1", OwnerUsername = "nointh", OwnerImage ="https://randomuser.me/api/portraits/men/51.jpg",
                //MediaList = new ObservableCollection<Media>
                //    {
                //        new Media{Type="image", Url="https://cdn.pixabay.com/photo/2019/05/03/14/24/landscape-4175978_960_720.jpg", VideoSource=""},
                //        new Media{Type="video", VideoSource="http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/BigBuckBunny.mp4", Url=""},
                //    },
                //PostImage = "https://cdn.pixabay.com/photo/2019/05/03/14/24/landscape-4175978_960_720.jpg"
                //, Caption = "Wowhoo best landscape ever",
                //PostTime = "12 min"},

                //new PostModel(){ PostId = "a2", OwnerUsername = "niax", OwnerImage ="https://randomuser.me/api/portraits/men/51.jpg",
                //PostImage = "https://helpx.adobe.com/content/dam/help/en/photoshop/using/convert-color-image-black-white/jcr_content/main-pars/before_and_after/image-before/Landscape-Color.jpg",
                //MediaList = new ObservableCollection<Media>
                //    {
                //        new Media{Type="image", Url="https://cdn.pixabay.com/photo/2020/11/09/01/46/leaves-5725356_960_720.png", VideoSource=""},
                //        new Media{Type="image", Url="https://cdn.pixabay.com/photo/2019/05/03/14/24/landscape-4175978_960_720.jpg", VideoSource=""},
                //    },
                //Caption = "Wowhoo best landscape ever",
                //PostTime = "12 min"},

                //new PostModel(){ PostId = "a3", OwnerUsername = "gen", OwnerImage ="https://randomuser.me/api/portraits/men/51.jpg",
                //PostImage = "https://cdn.pixabay.com/photo/2020/11/09/01/46/leaves-5725356_960_720.png",
                //MediaList = new ObservableCollection<Media>
                //    {
                //        new Media{Type="image", Url="https://cdn.pixabay.com/photo/2020/11/09/01/46/leaves-5725356_960_720.png", VideoSource=""},
                //        new Media{Type="image", Url="https://cdn.pixabay.com/photo/2019/05/03/14/24/landscape-4175978_960_720.jpg", VideoSource=""},
                //        new Media{Type="image", Url="https://cdn.pixabay.com/photo/2019/05/03/14/24/landscape-4175978_960_720.jpg", VideoSource=""},
                //        new Media{Type="image", Url="https://cdn.pixabay.com/photo/2019/05/03/14/24/landscape-4175978_960_720.jpg", VideoSource=""},
                //    },
                //Caption = "Wowhoo best landscape ever",
                //PostTime = "12 min"},

                //new PostModel(){ PostId = "a3", OwnerUsername = "gen", OwnerImage ="https://randomuser.me/api/portraits/men/51.jpg",
                //PostImage = "https://cdn.pixabay.com/photo/2020/06/02/19/37/coffee-5252373_960_720.jpg",
                //MediaList = new ObservableCollection<Media>
                //    {
                //        new Media{Type="image", Url="https://cdn.pixabay.com/photo/2020/11/09/01/46/leaves-5725356_960_720.png", VideoSource=""},
                //        new Media{Type="video", VideoSource="http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/VolkswagenGTIReview.mp4", Url=""},
                //        new Media{Type="video", VideoSource="http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/VolkswagenGTIReview.mp4", Url=""},
                //    },
                //Caption = "jaxckn nc jlan xc jcnxla  ncamxcjkan  a xc  ajs cxz ma,sn c  jasnj cxzmc,n noice cncmans  lnsakcnm   m s s alkm  d,as k al kasm sak dns",
                //PostTime = "12 min"}
            };
        }
    }

    class PostModelBasic
    {
        public string OwnerId { get; set; }
        public string Caption { get; set; }
        public string PostTime { get; set; }
    }

    public class Media : INotifyPropertyChanged
    {
        private string _Type;

        private ImageSource _Url;

        private MediaSource _VideoSource;

        public MediaSource VideoSource
        {
            get { return _VideoSource; }
            set
            {
                if (value != _VideoSource)
                {
                    _VideoSource = value;
                    OnPropertyChanged("VideoSource");
                }
            }
        }

        public ImageSource Url
        {
            get { return _Url; }
            set
            {
                if (value != _Url)
                {
                    _Url = value;
                    OnPropertyChanged("Url");
                }
            }
        }

        public string Type
        {
            get { return _Type; }
            set
            {
                if (value != _Type)
                {
                    _Type = value;
                    OnPropertyChanged("Type");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public static Media ParseContent(MediaContent content)
        {
            Media med = new Media();
            med.Type = content.Type;
            if (med.Type.ToLower() == "video")
            {
                med.VideoSource = content.Url;
                med.Url = "";
                return med;
            }
            med.VideoSource = "";
            med.Url = content.Url;
            return med;
        }
    }

    public class MediaContent
    {
        public string Type { get; set; }
        public string Url { get; set; }

        public MediaContent(string type, string url)
        {
            Type = type;
            Url = url;
        }

        public MediaContent() { }
    }
}
