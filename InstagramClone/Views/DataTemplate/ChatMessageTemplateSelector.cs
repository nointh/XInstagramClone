using InstagramClone.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace InstagramClone.Views.DataTemplate
{
    class ChatMessageTemplateSelector : DataTemplateSelector
    {
        public Xamarin.Forms.DataTemplate PeopleTemplate { get; set; }
        public Xamarin.Forms.DataTemplate SelfTemplate { get; set; }
        public ChatMessageTemplateSelector()
        {
            PeopleTemplate = new Xamarin.Forms.DataTemplate(typeof(PeopleMessageTemplate));
            SelfTemplate = new Xamarin.Forms.DataTemplate(typeof(SelfMessageTemplate));
        }
        protected override Xamarin.Forms.DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            ChatMessageWithFriendAvatar msg = (ChatMessageWithFriendAvatar)item;

            return msg.SenderID == FirebaseDB.CurrentUserId ? SelfTemplate : PeopleTemplate;
        }
    }
}
