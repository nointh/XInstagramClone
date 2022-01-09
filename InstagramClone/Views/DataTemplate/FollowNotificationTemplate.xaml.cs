using InstagramClone.Models;
using InstagramClone.Views.HomeTabbedPageViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InstagramClone.Views.DataTemplate
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FollowNotificationTemplate : ContentView
    {
        public FollowNotificationTemplate()
        {
            InitializeComponent();
            SetFollowStatus();
        }
        private async void SetFollowStatus()
        {
            //base.OnApplyTemplate();
            FirebaseDB db = new FirebaseDB();
            var noti = (NotificationModel)BindingContext;
            if (BindingContext == null)
                return;
            if (await db.checkIsFollow(FirebaseDB.CurrentUserId, noti.UserId))
            {
                FollowLabel.BackgroundColor = Color.Gray;
                FollowLabel.Text = "Followed";
            }
        }
        private async void FollowTap_Tapped(object sender, EventArgs e)
        {
            var noti = (NotificationModel)BindingContext;
            FirebaseDB db = new FirebaseDB();
            string result = await db.updateFollow(
                new FollowUser { UserKey = FirebaseDB.CurrentUserId },
                new FollowUser { UserKey = noti.UserId}
                );
            var blueColor = (Color)Application.Current.Resources["InsBlue"];
            if (result.Equals("unfollow"))
            {
                FollowLabel.BackgroundColor = blueColor;
                FollowLabel.Text = "Follow";
            }
            else
            {
                FollowLabel.BackgroundColor = Color.Gray;
                FollowLabel.Text = "Followed";
            }
        }

        private async void AvatarTap_Tapped(object sender, EventArgs e)
        {
            var noti = (NotificationModel)BindingContext;
            FirebaseDB firebase = new FirebaseDB();
            if (noti.UserId == FirebaseDB.CurrentUserId)
            {
                await Navigation.PushAsync(new YourProfile(await FirebaseDB.GetCurentUserInfo()));
            }
            else
                await Navigation.PushAsync(new ProfilePage(await firebase.getUser(noti.UserId), await FirebaseDB.GetCurentUserInfo()));

        }
    }
}