using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using InstagramClone.Views.HomeTabbedPageViews;
using InstagramClone.Views.ProfilePageViews;
using InstagramClone.Models;
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
        UserModel user;
        public HomeTabbedPage()
        {
            InitializeComponent();
            GetProfileInfoAndRefreshToken();
            NavigationPage homePage = new NavigationPage(new HomePage());
            NavigationPage discoveryPage = new NavigationPage(new DiscoveryPage());
            NavigationPage addPostPage = new NavigationPage(new AddPostPage());
            NavigationPage profilePage = new NavigationPage(new YourProfile(new UserModel() { UID = FirebaseDB.CurrentUserId }));
            Page notiPage = new LikePage();

            discoveryPage.IconImageSource = new FontImageSource { FontFamily = "FFASolid", Glyph = FontAwesomeIcons.Search };
            homePage.IconImageSource = new FontImageSource { FontFamily = "PFASolid", Glyph = FontAwesomeIcons.HomeAlt };
            addPostPage.IconImageSource = new FontImageSource { FontFamily = "FFARegular", Glyph = FontAwesomeIcons.PlusSquare };
            notiPage.IconImageSource = new FontImageSource { FontFamily = "FFARegular", Glyph = FontAwesomeIcons.Bell };
            profilePage.IconImageSource = new FontImageSource { FontFamily = "FFARegular", Glyph = FontAwesomeIcons.User };

            Children.Add(new HomePage());
            Children.Add(discoveryPage);
            Children.Add(addPostPage);
            Children.Add(notiPage);
            Children.Add(profilePage);
        }
        private async void GetProfileInfoAndRefreshToken()
        {
            var authProvider = FirebaseDB.GetAuthProvider();
            try
            {
                var savedFirebaseAuth = JsonConvert.DeserializeObject<Firebase.Auth.FirebaseAuth>(Preferences.Get("FirebaseRefreshToken", ""));
                var RefreshContent = await authProvider.RefreshAuthAsync(savedFirebaseAuth);
                FirebaseDB.CurrentUserId = RefreshContent.User.LocalId;
                Preferences.Set("FirebaseRefreshToken", JsonConvert.SerializeObject(RefreshContent));
                Preferences.Set("UID", RefreshContent.User.LocalId);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        
    }
}