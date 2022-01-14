using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Firebase.Auth;
using InstagramClone.Models;

namespace InstagramClone.Views.LoginPageViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignupPage : ContentPage
    {
        public SignupPage()
        {
            InitializeComponent();
            LogoImage.Source = ImageSource.FromResource("InstagramClone.Resources.Images.InstagramLogo.svg.png");
        }

        async private void BtnSignUp_Clicked(object sender, EventArgs e)
        {
            try
            {
                var authProvider = FirebaseDB.GetAuthProvider();
                var auth = await authProvider.CreateUserWithEmailAndPasswordAsync(TxtEmail.Text,TxtPassword.Text);
                string getToken = auth.FirebaseToken;
                string currentEmail = auth.User.Email;
                FirebaseDB db = new FirebaseDB();
                await db.addUser(new UserModel
                {
                    UID = auth.User.LocalId,
                    Email = TxtEmail.Text,
                    Fullname = TxtFullname.Text,
                    Username = TxtUsername.Text,
                    DOB = "",
                    Gender = "",
                    ImageUri = "https://firebasestorage.googleapis.com/v0/b/xinstagram-clone.appspot.com/o/avatar.png?alt=media&token=e076d091-4171-4de9-b43a-7a1f4ba60bcb",
                    Phone = "",
                    ProfileDescription = "",
                    Website = "",
                }) ;
                //await DisplayAlert("Alert", getToken, "OK");
                await DisplayAlert("Alert", "Sign up successfully", "OK");
                await Navigation.PushAsync(new LoginPage());
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                await DisplayAlert("Alert", "Invalid user or password", "OK");
            }

        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage( new LoginPage());
        }
    }
}