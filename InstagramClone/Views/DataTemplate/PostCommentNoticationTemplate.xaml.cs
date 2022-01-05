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
    public partial class PostCommentNoticationTemplate : ContentView
    {
        public string PostId;
        public PostCommentNoticationTemplate()
        {
            InitializeComponent();
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            PostId = ((NotificationModel)BindingContext).PostId;
            var noti = (NotificationModel)BindingContext;
            Navigation.PushAsync(new CommentPage(noti.UserId, noti.PostId));
        }

        private void Avatar_Tapped(object sender, EventArgs e)
        {
            PostId = ((NotificationModel)BindingContext).PostId;
            var noti = (NotificationModel)BindingContext;
            Navigation.PushAsync(new Page1(noti.UserId));
        }
    }
}