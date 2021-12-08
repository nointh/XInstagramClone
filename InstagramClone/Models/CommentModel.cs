using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace InstagramClone.Models
{
    class CommentModel : INotifyPropertyChanged
    {
        private string _CommentId;
        private string _Username;
        private string _UserImage;
        private string _CommentDetail;
        private string _PostTime;
        private string _OwnerId;

        public string CommentId
        {
            get { return _CommentId; }
            set
            {
                if (value != _CommentId)
                {
                    _CommentId = value;
                    OnPropertyChanged("CommentId");
                }
            }
        }
        public string Username
        {
            get { return _Username; }
            set
            {
                if (value != _Username)
                {
                    _Username = value;
                    OnPropertyChanged("Username");
                }
            }
        }
        public string UserImage
        {
            get { return _UserImage; }
            set
            {
                if (value != _UserImage)
                {
                    _UserImage = value;
                    OnPropertyChanged("UserImage");
                }
            }
        }
        public string CommentDetail
        {
            get { return _CommentDetail; }
            set
            {
                if (value != _CommentDetail)
                {
                    _CommentDetail = value;
                    OnPropertyChanged("CommentDetail");
                }
            }
        }
        public string PostTime
        {
            get { return _PostTime; }
            set
            {
                if (value != _PostTime)
                {
                    _PostTime = value;
                    OnPropertyChanged("PostTime");
                }
            }
        }
        public string OwnerId
        {
            get { return _OwnerId; }
            set
            {
                if (value != _OwnerId)
                {
                    _OwnerId = value;
                    OnPropertyChanged("OwnerId");
                }
            }
        }

        public CommentModel()
        {
            //PostId = "";
            CommentId = Username = UserImage = CommentDetail = PostTime = "";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    class FullCommentModel : INotifyPropertyChanged
    {
        private string _CommentId;
        private string _Username;
        private string _UserImage;
        private string _CommentDetail;
        private string _PostTime;
        private string _OwnerId;
        private List<UserLiked> _UserLiked;
        private int _LikeCount;

        public string CommentId
        {
            get { return _CommentId; }
            set
            {
                if (value != _CommentId)
                {
                    _CommentId = value;
                    OnPropertyChanged("CommentId");
                }
            }
        }
        public string Username
        {
            get { return _Username; }
            set
            {
                if (value != _Username)
                {
                    _Username = value;
                    OnPropertyChanged("Username");
                }
            }
        }
        public string UserImage
        {
            get { return _UserImage; }
            set
            {
                if (value != _UserImage)
                {
                    _UserImage = value;
                    OnPropertyChanged("UserImage");
                }
            }
        }
        public string CommentDetail
        {
            get { return _CommentDetail; }
            set
            {
                if (value != _CommentDetail)
                {
                    _CommentDetail = value;
                    OnPropertyChanged("CommentDetail");
                }
            }
        }
        public string PostTime
        {
            get { return _PostTime; }
            set
            {
                if (value != _PostTime)
                {
                    _PostTime = value;
                    OnPropertyChanged("PostTime");
                }
            }
        }
        public string OwnerId
        {
            get { return _OwnerId; }
            set
            {
                if (value != _OwnerId)
                {
                    _OwnerId = value;
                    OnPropertyChanged("OwnerId");
                }
            }
        }
        public List<UserLiked> UserLiked
        {
            get { return _UserLiked; }
            set
            {
                if (value != _UserLiked)
                {
                    _UserLiked = value;
                    OnPropertyChanged("UserLiked");
                }
            }
        }
        public int LikeCount
        {
            get { return _LikeCount; }
            set
            {
                if (value != _LikeCount)
                {
                    _LikeCount = value;
                    OnPropertyChanged("LikeCount");
                }
            }
        }

        public FullCommentModel()
        {
            //PostId = "";
            CommentId = Username = UserImage = CommentDetail = PostTime = "";
        }

        public FullCommentModel(CommentModel model)
        {
            CommentId = model.CommentId;
            Username = model.Username;
            UserImage = model.UserImage;
            CommentDetail = model.CommentDetail;
            PostTime = model.PostTime;
            OwnerId = model.OwnerId;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    class UserLiked
    {
        public string UserId { get; set; }
    }
}
