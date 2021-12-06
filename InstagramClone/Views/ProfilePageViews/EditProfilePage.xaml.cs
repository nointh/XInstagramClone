﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Database;

using InstagramClone.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InstagramClone.Views.ProfilePageViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditProfilePage : ContentPage
    {
        UserModel user;
        public EditProfilePage(UserModel user)
        {
            InitializeComponent();
            this.user = user;
            InitUserInformation();
        }

        private void InitUserInformation()
        {
            Title = user.Username;
            GenderPicker.ItemsSource = new string[]
            {
                "Male",
                "Female"
            };
            EntryName.Text = user.Fullname;
            EntryUsername.Text = user.Username;
            EntryWebsite.Text = user.Website;
            EntryEmail.Text = user.Email;
            EntryBio.Text = user.ProfileDescription;
            EntryPhone.Text = user.Phone;
            if (user.Gender == "Male")
            {
                GenderPicker.SelectedIndex = 0;
            } else
            {
                GenderPicker.SelectedIndex = 1;
            }
            string[] date = user.DOB.Split('/');
            PickerDOB.Date = new DateTime(Int32.Parse(date[2]), Int32.Parse(date[1]), Int32.Parse(date[0]));
        }

        async private void btnSave_Clicked(object sender, EventArgs e)
        {
            try
            {
                FirebaseDB db = new FirebaseDB();
                //Call the update profile
                UserModel user = new UserModel()
                {
                    Username = EntryUsername.Text,
                    Fullname = EntryName.Text,
                    ProfileDescription = EntryBio.Text,
                    Email = EntryEmail.Text,
                    Phone = EntryPhone.Text,
                    Gender = GenderPicker.SelectedItem.ToString(),
                    DOB = PickerDOB.Date.ToString("dd/MM/yyyy"),
                    ImageUri = this.user.ImageUri,
                    Website = EntryWebsite.Text,
                };
                await db.updateUser(user);
                await DisplayAlert("Notification", "Your profile has been updated!", "Ok");
                await Navigation.PopAsync();
            }
            catch (FirebaseException ex)
            {
                await DisplayAlert("Notification", "Error!", "Ok");
                await Navigation.PopAsync();
                Console.WriteLine(ex);
            }
        }
    }
}