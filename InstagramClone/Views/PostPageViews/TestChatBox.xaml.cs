using Firebase.Database.Query;
using InstagramClone.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InstagramClone.Views.PostPageViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TestChatBox : ContentPage
    {
        string chatboxId;
        string receiverId;
        ObservableCollection<ChatMessageWithFriendAvatar> messCollection = new ObservableCollection<ChatMessageWithFriendAvatar>();
        UserModel CurrentUser;
        public TestChatBox()
        {
            InitializeComponent();
        }
        public TestChatBox(string friendId)
        {
            InitializeComponent();
            receiverId = friendId;
            InitData(friendId);
        }
        public async void InitData(string friendId)
        {
            var list = await FirebaseDB.GetUserChatboxList();
            var currentChatBox = list.Where(i => i.ReceiverID == friendId).FirstOrDefault();
            if (currentChatBox == null)
            {
                chatboxId = await FirebaseDB.InsertChatBox(friendId);
            }
            else chatboxId = currentChatBox.ID;

            CurrentUser = await FirebaseDB.GetCurentUserInfo();
            lbFriendUsr.Text = (await FirebaseDB.GetUserById(receiverId)).Username;
          
            var result = FirebaseDB.firebaseClient
                .Child("chatmessage")
                .Child(chatboxId)
                .AsObservable<ChatMessageWithFriendAvatar>()
                .Subscribe(async (e) => {
                    if (e.Object != null)
                    {
                        await FirebaseDB.UpdateSeenForChatBox(chatboxId);
                        e.Object.ImageUri = CurrentUser.ImageUri;
                        //messCollection.Insert(messCollection.Count, e.Object);
                        messCollection.Add(e.Object);
                    }
                });
            MessageListview.ItemsSource = messCollection;
        }

        private async void lbSendMsg_Tapped(object sender, EventArgs e)
        {
            ChatMessage mess = new ChatMessage()
            {
                Message = editorMessage.Text,
                Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
                SenderID = CurrentUser.UID,
                SenderUsername = CurrentUser.Username,
            };
            await FirebaseDB.SendMessage(chatboxId, receiverId, mess);
        }
    }
}