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
    class GeneralChatBoxModel
    {

    }
}
