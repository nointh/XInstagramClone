using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace InstagramClone.Models
{
    public class UserModel
    {
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string Fullname { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string DOB { get; set; }
        public string Gender { get; set; }
        public string ImageUri { get; set; }

        //public int TotalFollower { get; set; }
        //public int TotalFollowing { get; set; }
        // public string ProfileDescription { get; set; }
        // public int TotalFollower { get; set; }
        // public int TotalFollowing { get; set; }
        public string ProfileDescription { get; set; }

        public UriImageSource ImageSource
        {
            get
            {
                return new UriImageSource()
                {
                    Uri = new Uri(ImageUri),
                    CachingEnabled = true,
                    CacheValidity = new TimeSpan(1, 0, 0, 0)
                };
            }
        }

    }
}
