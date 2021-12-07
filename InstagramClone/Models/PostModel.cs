using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace InstagramClone.Models
{
    class PostModel
    {
        public string PostId { get; set; }
        public string OwnerUsername { get; set; }
        public string OwnerImage { get; set; }
        public string PostImage { get; set; }
        public ObservableCollection<Media> MediaList { get; set; }
        [JsonProperty("Caption")]
        public string Caption { get; set; }
        public string PostTime { get; set; }
        public bool IsLike { get; set; }
        public PostModel()
        {
            PostId = "";
            PostImage = "";
            IsLike = false;
        }
        public static ObservableCollection<PostModel> GetExamplePostList()
        {
            return new ObservableCollection<PostModel>()
            {
                new PostModel(){ PostId = "a1", OwnerUsername = "nointh", OwnerImage ="https://randomuser.me/api/portraits/men/51.jpg",
                MediaList = new ObservableCollection<Media>
                    {
                        new Media{Type="image", Url="https://cdn.pixabay.com/photo/2019/05/03/14/24/landscape-4175978_960_720.jpg"},
                        new Media{Type="video", Url="http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/BigBuckBunny.mp4"},
                    },
                PostImage = "https://cdn.pixabay.com/photo/2019/05/03/14/24/landscape-4175978_960_720.jpg"
                , Caption = "Wowhoo best landscape ever",
                PostTime = "12 min"},

                new PostModel(){ PostId = "a2", OwnerUsername = "niax", OwnerImage ="https://randomuser.me/api/portraits/men/51.jpg",
                PostImage = "https://helpx.adobe.com/content/dam/help/en/photoshop/using/convert-color-image-black-white/jcr_content/main-pars/before_and_after/image-before/Landscape-Color.jpg",
                MediaList = new ObservableCollection<Media>
                    {
                        new Media{Type="image", Url="https://cdn.pixabay.com/photo/2020/11/09/01/46/leaves-5725356_960_720.png"},
                        new Media{Type="image", Url="https://cdn.pixabay.com/photo/2019/05/03/14/24/landscape-4175978_960_720.jpg"},
                    },
                Caption = "Wowhoo best landscape ever",
                PostTime = "12 min"},

                new PostModel(){ PostId = "a3", OwnerUsername = "gen", OwnerImage ="https://randomuser.me/api/portraits/men/51.jpg",
                PostImage = "https://cdn.pixabay.com/photo/2020/11/09/01/46/leaves-5725356_960_720.png",
                MediaList = new ObservableCollection<Media>
                    {
                        new Media{Type="image", Url="https://cdn.pixabay.com/photo/2020/11/09/01/46/leaves-5725356_960_720.png"},
                        new Media{Type="image", Url="https://cdn.pixabay.com/photo/2019/05/03/14/24/landscape-4175978_960_720.jpg"},
                        new Media{Type="image", Url="https://cdn.pixabay.com/photo/2019/05/03/14/24/landscape-4175978_960_720.jpg"},
                        new Media{Type="image", Url="https://cdn.pixabay.com/photo/2019/05/03/14/24/landscape-4175978_960_720.jpg"},
                    },
                Caption = "Wowhoo best landscape ever",
                PostTime = "12 min"},

                new PostModel(){ PostId = "a3", OwnerUsername = "gen", OwnerImage ="https://randomuser.me/api/portraits/men/51.jpg",
                PostImage = "https://cdn.pixabay.com/photo/2020/06/02/19/37/coffee-5252373_960_720.jpg",
                MediaList = new ObservableCollection<Media>
                    {
                        new Media{Type="image", Url="https://cdn.pixabay.com/photo/2020/11/09/01/46/leaves-5725356_960_720.png"},
                        new Media{Type="video", Url="http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/VolkswagenGTIReview.mp4"},
                        new Media{Type="video", Url="http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/VolkswagenGTIReview.mp4"},
                    },
                Caption = "jaxckn nc jlan xc jcnxla  ncamxcjkan  a xc  ajs cxz ma,sn c  jasnj cxzmc,n noice cncmans  lnsakcnm   m s s alkm  d,as k al kasm sak dns",
                PostTime = "12 min"}
            };
        }
        public class Media
        {
            public string Type { get; set; }
            public string Url { get; set; }

        }
<<<<<<< Updated upstream
        
=======
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
            med.Url = content.Url;
            return med;
        }
    }
    public class MediaContent
    {
        public string Type { get; set; }
        public string Url { get; set; }
>>>>>>> Stashed changes
    }
    
}
