using InstagramClone.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace InstagramClone.Views.DataTemplate
{
    public class NotificationTemplateSelector : DataTemplateSelector
    {
        public Xamarin.Forms.DataTemplate PostComment { get; set; }
        public Xamarin.Forms.DataTemplate PostLike { get; set; }
        public Xamarin.Forms.DataTemplate Follow { get; set; }
        public Xamarin.Forms.DataTemplate Empty { get; set; }
        private bool isFollowed = false;
        public NotificationTemplateSelector()
        {
            PostComment = new Xamarin.Forms.DataTemplate(typeof(PostCommentNoticationTemplate));
            PostLike = new Xamarin.Forms.DataTemplate(typeof(PostLikeNotificationTemplate));
            Follow = new Xamarin.Forms.DataTemplate(typeof(FollowNotificationTemplate));
            Empty = new Xamarin.Forms.DataTemplate(typeof(EmptyTemplate));
        }
        protected override Xamarin.Forms.DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            NotificationModel noti = (NotificationModel)item;
            //PickMediaItem media = (PickMediaItem)item;
            Task.Run(async () =>
            {   
                FirebaseDB db = new FirebaseDB();
                isFollowed = await db.checkIsFollow(noti.UserId, FirebaseDB.CurrentUserId);
            });

            switch(noti.Type.ToLower())
            {
                case "postcomment":
                    return PostComment;
                case "postlike":
                    return PostLike;
                case "follow":
                    //if (isFollowed)
                    //    return Empty;
                    return Follow;
                default:
                    return PostLike;
            }
        }
        async void CheckIsFollow()
        {

        }
    }
}
