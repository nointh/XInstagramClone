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
        public List<string> PostImages { get; set; }
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
                new PostModel(){ PostId = "1", OwnerUsername = "nointh", OwnerImage ="https://randomuser.me/api/portraits/men/51.jpg",
                PostImage = "https://cdn.pixabay.com/photo/2019/05/03/14/24/landscape-4175978_960_720.jpg"
                , Caption = "Wowhoo best landscape ever",
                PostTime = "12 min"},
                new PostModel(){ PostId = "2", OwnerUsername = "niax", OwnerImage ="https://randomuser.me/api/portraits/men/51.jpg",
                PostImage = "https://helpx.adobe.com/content/dam/help/en/photoshop/using/convert-color-image-black-white/jcr_content/main-pars/before_and_after/image-before/Landscape-Color.jpg"
                , Caption = "Wowhoo best landscape ever",
                PostTime = "12 min"},
                new PostModel(){ PostId = "3", OwnerUsername = "gen", OwnerImage ="https://randomuser.me/api/portraits/men/51.jpg",
                PostImage = "https://cdn.pixabay.com/photo/2020/11/09/01/46/leaves-5725356_960_720.png"
                , Caption = "Wowhoo best landscape ever",
                PostTime = "12 min"},
                new PostModel(){ PostId = "3", OwnerUsername = "gen", OwnerImage ="https://randomuser.me/api/portraits/men/51.jpg",
                PostImage = "https://cdn.pixabay.com/photo/2020/06/02/19/37/coffee-5252373_960_720.jpg"
                , Caption = "jaxckn nc jlan xc jcnxla  ncamxcjkan  a xc  ajs cxz ma,sn c  jasnj cxzmc,n noice cncmans  lnsakcnm   m s s alkm  d,as k al kasm sak dns",
                PostTime = "12 min"}
            };
        }
    }
}
