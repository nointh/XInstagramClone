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
        ObservableCollection<ChatboxModelWithReceiverInfo> chatboxCollection = new ObservableCollection<ChatboxModelWithReceiverInfo>();
        
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
                .AsObservable<ChatboxModelWithReceiverInfo>()
                .Subscribe((e) => {
                    if (e.Object != null)
                    {
                        if (e.EventType == Firebase.Database.Streaming.FirebaseEventType.InsertOrUpdate)
                        {
                            List<ChatboxModelWithReceiverInfo> list = chatboxCollection.ToList();
                            ChatboxModelWithReceiverInfo chat = list.Where(i => i.ID == e.Key).FirstOrDefault() ;

                            var userInfo = FirebaseDB.GetUserById(e.Object.ReceiverID);
                            e.Object.ImageUri = userInfo.Result.ImageUri;
                            e.Object.Fullname = userInfo.Result.Fullname;
                            if (e.Object.IsRead)
                            {
                                e.Object.LastMessageColor = "DimGray";
                            }
                            else e.Object.LastMessageColor = "Black";

                            if (chat == null) //if not in chatboxCollection yet, insert it then
                            {
                                e.Object.ID = e.Key;
                                if (chatboxCollection.Count > 0
                                && 
                                CompareTimeString(e.Object.UpdateAt, chatboxCollection[chatboxCollection.Count - 1].UpdateAt)
                                )
                                {
                                    chatboxCollection.Insert(0, e.Object);
                                }    
                                else chatboxCollection.Add(e.Object);
                            }
                            else // else meaning that chatbox already has that chatbox model
                            {
                                list.Remove(chat);
                                e.Object.ID = e.Key;
                                list.Insert(0, e.Object);
                                chatboxCollection.Clear();
                                foreach(var item in list)
                                {
                                    chatboxCollection.Add(item);
                                }
                            }
                        }
                    }
                }); 
        }
        private bool CompareTimeString(string time1, string time2)
        {
            try
            {
                DateTime datetime1 = DateTime.Parse(time1);
                DateTime datetime2 = DateTime.Parse(time2);
                return DateTime.Compare(datetime1, datetime2) > 0;
            }
            catch
            {
                return false;
            }
        }
        private void ChatboxListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ChatboxModelWithReceiverInfo chatmodel = ChatboxListView.SelectedItem as ChatboxModelWithReceiverInfo;
            Navigation.PushAsync(new TestChatBox(chatmodel.ReceiverID));
        }
    }
}