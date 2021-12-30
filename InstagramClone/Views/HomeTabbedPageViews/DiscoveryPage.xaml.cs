using InstagramClone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InstagramClone.Views.HomeTabbedPageViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DiscoveryPage : ContentPage
    {
        List<SearchUserModel> list = new List<SearchUserModel>();

        public DiscoveryPage()
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
                list = new List<SearchUserModel>();
            }
            lsvAccounts.ItemsSource = list;
        }

        private void lsvAccounts_ItemTapped(object sender, ItemTappedEventArgs e)
        {

        }
    }
}