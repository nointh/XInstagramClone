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
    public partial class TestChatboxList : ContentPage
    {
        ObservableCollection<UserChatboxModel> chatboxCollection = new ObservableCollection<UserChatboxModel>();
        public TestChatboxList()
        {
            InitializeComponent();
            initData();
        }
        public void initData()
        {
            ChatboxListView.ItemsSource = chatboxCollection;
            var result = FirebaseDB.firebaseClient
                .Child("userchat")
                .Child(FirebaseDB.CurrentUserId)
                .AsObservable<UserChatboxModel>()
                .Subscribe((e) => {
                    if (e.Object != null)
                    {
                        if (e.EventType == Firebase.Database.Streaming.FirebaseEventType.InsertOrUpdate)
                        {
                            List<UserChatboxModel> list = chatboxCollection.ToList();
                            UserChatboxModel chat = list.Where(i => i.ID == e.Key).FirstOrDefault() ;

                            if (chat == null) //if not in chatboxCollection yet, insert it then
                            {
                                e.Object.ID = e.Key;
                                chatboxCollection.Insert(0, e.Object);
                            }
                            else // else meaning that chatbox already has that chatbox model
                            {
                                list.Remove(chat);
                                e.Object.ID = e.Key;
                                list.Insert(0, e.Object);
                                chatboxCollection.Clear();
                                foreach(var item in list)
                                {
                                    chatboxCollection.Insert(0, item);
                                }
                            }
                        }
                    }
                });
        }

        private void ChatboxListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            UserChatboxModel chatmodel = ChatboxListView.SelectedItem as UserChatboxModel;
            Navigation.PushAsync(new TestChatBox(chatmodel.ReceiverID));
        }
    }
}