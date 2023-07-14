using Land_listing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Land_listing.Views.UserView
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdministratorDashboardView : ContentPage
    {
        public AdministratorDashboardView( User user)
        {
            InitializeComponent();
            hello.Text = user.Username;
            List<CarouselImage> images = new List<CarouselImage>()
            {
                new CarouselImage {image = "image1.png"},
                new CarouselImage {image = "image2.png"},
                new CarouselImage {image = "image3.png"}
            };
           carousel.ItemsSource = images;
            Device.StartTimer(TimeSpan.FromSeconds(2), (Func<bool>)(() =>
            {
                carousel.Position = (carousel.Position + 1) % images.Count();
                return true;
            }));
        }

        private async void GetUsers_Tapped(object sender, EventArgs e)
        {
            // navigate to user page
            await Shell.Current.Navigation.PushAsync(new userPage());
        }
    }
}