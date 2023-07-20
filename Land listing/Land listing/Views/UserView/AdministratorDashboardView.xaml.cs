using Land_listing.Models;
using Land_listing.Views.LandView;
using SkiaSharp;
using Microcharts;
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
        private User getUser;
        public AdministratorDashboardView( User user)
        {
            InitializeComponent();
           // hello.Text = user.Username;
           // List<CarouselImage> images = new List<CarouselImage>()
           // {
           //     new CarouselImage {image = "image1.png"},
           //     new CarouselImage {image = "image2.png"},
           //     new CarouselImage {image = "image3.png"}
           // };
           //carousel.ItemsSource = images;
           // Device.StartTimer(TimeSpan.FromSeconds(2), (Func<bool>)(() =>
           // {
           //     carousel.Position = (carousel.Position + 1) % images.Count();
           //     return true;
           // }));
           getUser = user;
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            onDashboardAppearance();
            List<ChartEntry> entries = new List<ChartEntry>
            {
                  new ChartEntry(212)
                {
                    Label = "land sites",
                    ValueLabel = "112",
                    Color = SKColor.Parse("#fcbf49")
                },
                new ChartEntry(248)
                {
                    Label = "User Request",
                    ValueLabel = "648",
                    Color = SKColor.Parse("#77d065")
                },
                new ChartEntry(128)
                {
                    Label = "Land sold",
                    ValueLabel = "428",
                    Color = SKColor.Parse("#b455b6")
                },
                new ChartEntry(514)
                {
                    Label = "Total User",
                    ValueLabel = "214",
                    Color = SKColor.Parse("#3498db")
                }
            };
            TimeSpan ts = new TimeSpan(0, 0, 0, 5);
            chartView.Chart = new PieChart { Entries = entries, IsAnimated = true, AnimationDuration = ts, AnimationProgress = 5, LabelTextSize = 30 };
        }

        private void DashBoard_Tapped(object sender, EventArgs e)
        {
            dashlbl.TextColor = Color.White;
            framedash.BackgroundColor = Color.MidnightBlue;
            frameView.IsVisible = true;

            statuslbl.TextColor = Color.MidnightBlue;
            framestatus.BackgroundColor = Color.White;
            chartView.IsVisible = false;
        }

        private void Status_Tapped(object sender, EventArgs e)
        {

            statuslbl.TextColor = Color.White;
            framestatus.BackgroundColor = Color.MidnightBlue;
            chartView.IsVisible = true;

            dashlbl.TextColor = Color.MidnightBlue;
            framedash.BackgroundColor = Color.White;
            frameView.IsVisible = false;
        }
        public void onDashboardAppearance()
        {
            dashlbl.TextColor = Color.White;
            framedash.BackgroundColor = Color.MidnightBlue;
            frameView.IsVisible = true;

            statuslbl.TextColor = Color.MidnightBlue;
            framestatus.BackgroundColor = Color.White;
            chartView.IsVisible = false;
        }

        private async void GoToLands_Tapped(object sender, EventArgs e)
        {
            // navigate to land page
              await Shell.Current.Navigation.PushAsync(new Lands(getUser));
        }

        private async void GoToUsers_Tapped(object sender, EventArgs e)
        {
            // navigate to user page
              await Shell.Current.Navigation.PushAsync(new userPage());
        }
        //private async void GetUsers_Tapped(object sender, EventArgs e)
        //{
        //    // navigate to user page
        //    await Shell.Current.Navigation.PushAsync(new userPage());
        //}
        //private async void Getlands_Tapped(object sender, EventArgs e)
        //{
        //    // navigate to land page
        //    await Shell.Current.Navigation.PushAsync(new Lands(getUser));
        //}

        //private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        //{
        //    // navigate to land page
        //    //await Shell.Current.Navigation.PushAsync(new Lands());
        //}
    }
}