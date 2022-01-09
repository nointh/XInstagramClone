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
        ObservableCollection<ChatMessage> messCollection = new ObservableCollection<ChatMessage>();
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
            MessListview.ItemsSource = messCollection;
            var result = FirebaseDB.firebaseClient
                .Child("chatmessage")
                .Child(chatboxId)
                .AsObservable<ChatMessage>()
                .Subscribe(async (e) => {
                    if (e.Object != null)
                    {
                        await FirebaseDB.UpdateSeenForChatBox(chatboxId);
                        messCollection.Insert(0, e.Object);
                    }
                });
        }

        private async void Enter_Clicked(object sender, EventArgs e)
        {
            ChatMessage mess = new ChatMessage()
            {
                Message = MessageInput.Text,
                Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
                SenderID = CurrentUser.UID,
                SenderUsername = CurrentUser.Username,
            };
            await FirebaseDB.SendMessage(chatboxId, receiverId, mess);
        }
    }
}