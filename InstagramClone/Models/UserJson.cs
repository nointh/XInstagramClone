using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace InstagramClone.Models
{
    class UserJson
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Fullname { get; set; }
        public string Username { get; set; }
        public UserJson GetValue()
        {
            return new UserJson
            {
                Email = this.Email,
                Fullname = this.Fullname,
                Username = this.Username
            };
        }    
    }

    class SearchUserModel
    {
        public string Id { get; set; }
        public string ImageUri { get; set; }
        public string Fullname { get; set; }
        public string Username { get; set; }
    }
    class FollowUserModel : INotifyPropertyChanged
    {
        public string Id { get; set; }
        public string ImageUri { get; set; }
        public string Fullname { get; set; }
        public string Username { get; set; }
        private bool _isFollowed = false;
        public bool IsFollowed
        {
            get { return _isFollowed; }
            set
            {
                if (value != _isFollowed)
                {
                    _isFollowed = value;
                    OnPropertyChanged("IsFollowed");
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

    }
}
