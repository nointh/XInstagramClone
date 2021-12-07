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
    public partial class WriteCaptionPage : ContentPage
    {
        public WriteCaptionPage()
        {
            InitializeComponent();
        }

        public WriteCaptionPage(Media firstItem, int quantity)
        {
            InitializeComponent();
            initViews(firstItem, quantity);
        }

        private void btnBack_Tapped(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void initViews(Media firstItem, int quantity)
        {
            //Frame items quantity
            if (quantity > 1)
            {
                lbQuantity.Text = "+" + (quantity - 1).ToString();
            }
            else
            {
                frQuantity.IsVisible = false;
            }

            //Frame img/video
            if (firstItem.Type == "video")
            {
                imgFirstItem.Source = "https://media.istockphoto.com/vectors/video-clip-player-icon-black-minimalist-icon-isolated-on-white-vector-id867290694?k=20&m=867290694&s=170667a&w=0&h=OYq8MB-wj9o1r2_w6-c8AKOlSf4S2PyE8Z3rzzbuWEk=";
            }
            else
            {
                imgFirstItem.Source = firstItem.Url;
            }
        }
    }
}