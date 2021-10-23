using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using InstagramClone.Models;

namespace InstagramClone.Views.HomeTabbedPageViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();
            LogoImage.Source = ImageSource.FromResource("InstagramClone.Resources.Images.InstagramLogo.svg.png");
            initData();
        }
        public void initData()
        {
            List<UserModel> list = new List<UserModel>();
            list.Add(new UserModel { Username = "ntncxzcaaaaaaaaaaaaaaaaaaaaaaaa", Fullname = "Nguyen Thanh Noi", ImageUri = "https://randomuser.me/api/portraits/men/51.jpg" });
            list.Add(new UserModel { Username = "Shizune", Fullname = "Nguyen Thanh Noi", ImageUri = "https://randomuser.me/api/portraits/women/51.jpg" });
            list.Add(new UserModel { Username = "Ngthclone", Fullname = "Nguyen Thanh Noi", ImageUri = "https://randomuser.me/api/portraits/men/26.jpg" });
            list.Add(new UserModel { Username = "Andre", Fullname = "Nguyen Thanh Noi", ImageUri = "https://randomuser.me/api/portraits/women/64.jpg" });
            list.Add(new UserModel { Username = "Taka", Fullname = "Nguyen Thanh Noi", ImageUri = "https://randomuser.me/api/portraits/men/72.jpg" });

            CollViewStory.ItemsSource = list;
        }
    }
}