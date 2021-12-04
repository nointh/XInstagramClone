using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace InstagramClone.Models
{
    public class StoryCollectionModel
    {
        public string CollectionId { get; set; }
        public string OwnerUsername { get; set; }
        public string CoverImage { get; set; }
        public List<string> Images { get; set; }
        public string Title { get; set; }

        public UriImageSource ImageSource
        {
            get
            {
                return new UriImageSource()
                {
                    Uri = new Uri(CoverImage),
                    CachingEnabled = true,
                    CacheValidity = new TimeSpan(1, 0, 0, 0)
                };
            }
        }

    }
}
