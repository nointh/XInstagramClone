using InstagramClone.Models;
using InstagramClone.Views.HomeTabbedPageViews;
using InstagramClone.Views.PostPageViews;
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
    public partial class PostLikeNotificationTemplate : ContentView
    {
        public string PostId;
        public PostLikeNotificationTemplate()
        {
            InitializeComponent();
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            PostId = ((NotificationModel)BindingContext).PostId;
            Navigation.PushAsync(new PostDetailPage(PostId));
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
        //protected override void OnBindingContextChanged()
        //{
        //    base.OnBindingContextChanged();
        //    if (BindingContext != null)
        //    {
        //        PostId = ((NotificationModel)BindingContext).PostId;
        //    }
        //}
    }
}