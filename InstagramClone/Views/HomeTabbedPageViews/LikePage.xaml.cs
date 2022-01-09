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
        ObservableCollection<NotificationModel> notificationCollection;
        public LikePage()
        {
            InitializeComponent();
            BindingContext = this;
            notificationCollection = new ObservableCollection<NotificationModel>();
            LoadData();
            MessageList.ItemsSource = notificationCollection;

        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
        public async void LoadData()
        {
            var collection = FirebaseDB.firebaseClient
                .Child("notification")
                .Child(FirebaseDB.CurrentUserId)
                .AsObservable<NotificationModel>()
                .Subscribe((e) =>
                {
                    if (e.Object != null)
                    {
                        e.Object.Id = e.Key;
                        notificationCollection.Insert(0, e.Object);
                    }
                });
            //await FirebaseDB.SetNotificationRealTimeListenter(notificationCollection);
        }
    }
}