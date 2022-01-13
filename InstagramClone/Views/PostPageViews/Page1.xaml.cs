using InstagramClone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InstagramClone.Views.PostPageViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page1 : ContentPage
    {
        List<SearchUserModel> list = new List<SearchUserModel>();

        public Page1()
        {
            InitializeComponent();
        }
        private void btnClearInput_Tapped(object sender, EventArgs e)
        {
            entrySearch.Text = "";
        }

        private async void entrySearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (entrySearch.Text.Length > 0)
            {
                lbClearText.IsVisible = true;

                var result = await FirebaseDB.GetUserOnSearchInput(entrySearch.Text);
                list = result;
            }
            else
            {
                lbClearText.IsVisible = false;
                GoToChatBox.IsVisible = true;
                list = new List<SearchUserModel>();
            }
            lsvAccounts.ItemsSource = list;
        }

        private void lsvAccounts_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            SearchUserModel user = lsvAccounts.SelectedItem as SearchUserModel;
            //DisplayAlert("info", "this user id is " + user.Id, "ok");
            Navigation.PushAsync(new TestChatBox(user.Id));
        }

        private void GoToChatBox_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new TestChatboxList());
        }

        private void entrySearch_Focused(object sender, FocusEventArgs e)
        {
            GoToChatBox.IsVisible = false;
        }
    }
}