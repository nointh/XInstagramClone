using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InstagramClone.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InstagramClone.Views.ProfilePageViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditProfilePage : ContentPage
    {
        public EditProfilePage(UserModel user)
        {
            InitializeComponent();
            InitUserInformation(user);
        }

        private void InitUserInformation(UserModel user)
        {
            EntryName.Text = user.Fullname;
            EntryUsername.Text = user.Username;
            EntryWebsite.Text = user.Website;
            EntryEmail.Text = user.Email;
            EntryBio.Text = user.ProfileDescription;
            EntryPhone.Text = user.Phone;
            GenderPicker.Title = user.Gender;
            //EntryDOB.Text = user.DOB;
        }

        private void btnSave_Clicked(object sender, EventArgs e)
        {
            //Call the update profile
            DisplayAlert("Notification", "Your profile has been updated!", "Ok");
            Navigation.PopAsync();
        }
    }
}