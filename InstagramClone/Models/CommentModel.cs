using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace InstagramClone.Models
{
    class CommentModel
    {
        public string CommentId { get; set; }
        public string PostId { get; set; }
        public string Username { get; set; }
        public string UserImage { get; set; }
        public string CommentDetail { get; set; }
        public string PostTime { get; set; }
        public int LikeCount { get; set; }
        public bool IsLike { get; set; }
        public CommentModel()
        {
            PostId = "";
            CommentId = "";
            IsLike = false;
        }

        public static ObservableCollection<CommentModel> GetExampleCommentList()
        {
            return new ObservableCollection<CommentModel>()
            {
               new CommentModel(){ CommentId = "1", PostId = "1", Username = "nointh", UserImage ="https://randomuser.me/api/portraits/men/51.jpg",
                CommentDetail = "wow, fantastic photo!", PostTime = "12min", LikeCount = 4, IsLike = false},
               new CommentModel(){ CommentId = "2", PostId = "1", Username = "dungtd", UserImage ="https://randomuser.me/api/portraits/men/51.jpg",
                CommentDetail = "post more of your photos please!", PostTime = "2d", LikeCount = 1, IsLike = false},
               new CommentModel(){ CommentId = "3", PostId = "1", Username = "danhmc1252", UserImage ="https://randomuser.me/api/portraits/men/51.jpg",
                CommentDetail = "🔥❤️❤️!", PostTime = "1d", LikeCount = 3, IsLike = false},
               new CommentModel(){ CommentId = "1", PostId = "1", Username = "nointh", UserImage ="https://randomuser.me/api/portraits/men/51.jpg",
                CommentDetail = "wow, fantastic photo!", PostTime = "12min", LikeCount = 4, IsLike = false},
               new CommentModel(){ CommentId = "2", PostId = "1", Username = "dungtd", UserImage ="https://randomuser.me/api/portraits/men/51.jpg",
                CommentDetail = "post more of your photos please!", PostTime = "2d", LikeCount = 1, IsLike = false},
               new CommentModel(){ CommentId = "3", PostId = "1", Username = "danhmc1252", UserImage ="https://randomuser.me/api/portraits/men/51.jpg",
                CommentDetail = "🔥❤️❤️!", PostTime = "1d", LikeCount = 3, IsLike = false},
               new CommentModel(){ CommentId = "1", PostId = "1", Username = "nointh", UserImage ="https://randomuser.me/api/portraits/men/51.jpg",
                CommentDetail = "wow, fantastic photo!", PostTime = "12min", LikeCount = 4, IsLike = false},
               new CommentModel(){ CommentId = "2", PostId = "1", Username = "dungtd", UserImage ="https://randomuser.me/api/portraits/men/51.jpg",
                CommentDetail = "post more of your photos please!", PostTime = "2d", LikeCount = 1, IsLike = false},
               new CommentModel(){ CommentId = "3", PostId = "1", Username = "danhmc1252", UserImage ="https://randomuser.me/api/portraits/men/51.jpg",
                CommentDetail = "🔥❤️❤️!", PostTime = "1d", LikeCount = 3, IsLike = false},
            };
        }
    }
}
