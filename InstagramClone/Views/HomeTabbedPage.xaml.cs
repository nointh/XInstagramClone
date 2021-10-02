using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using InstagramClone.Views.HomeTabbedPageViews;
using FontAwesome;
using Xamarin.Essentials;
using Newtonsoft.Json;
using Firebase.Auth;

namespace InstagramClone.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomeTabbedPage : TabbedPage
    {
        readonly private string WebAPIKey = "AIzaSyCc-Lrg3ue3OTaFHfYhtQZtgvQZHtsJAUs";
        public HomeTabbedPage()
        {
            InitializeComponent();

            NavigationPage homePage = new NavigationPage(new HomePage()) { BarBackgroundColor = Color.White };
            homePage.IconImageSource = new FontImageSource { FontFamily = "PFASolid", Glyph = FontAwesomeIcons.HomeAlt };
            Children.Add(homePage);
            Children.Add(new DiscoveryPage());
            Children.Add(new AddPostPage());
            Children.Add(new LikePage());
            Children.Add(new ProfilePage());
            GetProfileInfoAndRefreshToken();
        }
        async private void GetProfileInfoAndRefreshToken()
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebAPIKey));
            try
            {
                var savedFirebaseAuth = JsonConvert.DeserializeObject<Firebase.Auth.FirebaseAuth>(Preferences.Get("FirebaseRefreshToken", ""));
                var RefreshContent = await authProvider.RefreshAuthAsync(savedFirebaseAuth);
                Preferences.Set("FirebaseRefreshToken", JsonConvert.SerializeObject(RefreshContent));

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                await DisplayAlert("Alert", "Token expired", "OK");
            }
        }
    }
}