using Firebase.Database.Query;
using InstagramClone.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InstagramClone.Views.HomeTabbedPageViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LikePage : ContentPage
    {
        ObservableCollection<NotificationModel> notificationCollection = new ObservableCollection<NotificationModel>();
        public LikePage()
        {
            InitializeComponent();
            BindingContext = this;
            Task.Run(LoadData);
            Console.WriteLine(notificationCollection.Count);
            MessageList.ItemsSource = notificationCollection;

        }
        public async Task LoadData()
        {
            var collection = FirebaseDB.firebaseClient
                .Child("notification")
                .Child(FirebaseDB.CurrentUserId)
                .AsObservable<NotificationModel>()
                .Subscribe((e) =>{ 
                    if(e.Object != null)
                    {
                        notificationCollection.Insert(0, e.Object);
                    }
                });
        }
    }
}