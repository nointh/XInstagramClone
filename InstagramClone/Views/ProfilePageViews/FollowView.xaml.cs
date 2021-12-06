using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InstagramClone.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InstagramClone.Views.ProfilePageViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FollowView : ContentPage
    {
        List<FollowUser> Follower;
        List<FollowUser> Following;
        public FollowView(List<FollowUser> Follower, List<FollowUser> Following)
        {
            InitializeComponent();
            this.Follower = Follower;
            this.Following = Following;
            InitFollow();
        }

        private void InitFollow()
        {
            FollowerList.ItemsSource = Follower;
            FollowingList.ItemsSource = Following;
        }
    }
}