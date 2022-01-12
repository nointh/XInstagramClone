using System;
using System.Collections.Generic;
using System.Text;

namespace InstagramClone.Models
{
    class UserChatboxModel
    {
        public string ID { get; set; }
        public string ReceiverID { get; set; }
        public bool IsRead { get; set; }
        public string LastMessage { get; set; }
        public string UpdateAt { get; set; }
    }
    class ChatMessage
    {
        public string SenderID { get; set; }
        public string SenderUsername { get; set; }
        public string Time { get; set; }
        public string Message { get; set; }
        public string PostID { get; set; }
    }
    class ChatMessageWithFriendAvatar
    {
        public string SenderID { get; set; }
        public string SenderUsername { get; set; }
        public string ImageUri { get; set; }
        public string Time { get; set; }
        public string Message { get; set; }
        public string PostID { get; set; }
    }
    class GeneralChatBoxModel
    {

    }
    class ChatboxModelWithReceiverInfo
    {
        public string ID { get; set; }
        public string ReceiverID { get; set; }
        public bool IsRead { get; set; }
        public string LastMessage { get; set; }
        public string UpdateAt { get; set; }
        public string ImageUri { get; set; }
        public string Fullname { get; set; }
        public string LastMessageColor { get; set; }

        public ChatboxModelWithReceiverInfo()
        {
            
        }

        public ChatboxModelWithReceiverInfo(UserChatboxModel cb, SearchUserModel sr)
        {
            this.ID = cb.ID;
            this.ReceiverID = cb.ReceiverID;
            this.IsRead = cb.IsRead;
            this.LastMessage = cb.LastMessage;
            this.UpdateAt = cb.UpdateAt;
            this.ImageUri = sr.ImageUri;
            this.Fullname = sr.Fullname;
            this.LastMessageColor = "DimGray";
        }
    }
}
