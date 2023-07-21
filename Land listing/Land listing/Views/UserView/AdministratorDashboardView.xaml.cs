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
using Land_listing.Services;

namespace Land_listing.Views.UserView
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdministratorDashboardView : ContentPage
    {
        public IDataUser<Models.User> dataUser { get; set; }
        public IdataLand<Models.Land> dataLand { get; set; }
        public IdataUserland<User_Land> dataUserLand { get; set; }
        private User getUser;
        public AdministratorDashboardView( User user)
        {
            InitializeComponent();
            dataUser = DependencyService.Get<IDataUser<Models.User>>();
            dataLand = DependencyService.Get<IdataLand<Models.Land>>();
            dataUserLand = DependencyService.Get<IdataUserland<User_Land>>();
            
            getUser = user;
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            onDashboardAppearance();
            DashboardFigure();
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
            dashlbl.TextColor = Color.FromHex("#2f3e46");
            framedash.BorderColor = Color.FromHex("#84a59d");
            frameView.IsVisible = true;

            statuslbl.TextColor = Color.FromHex("#84a59d");
            framestatus.BorderColor = Color.White;
            chartView.IsVisible = false;
        }

        private void Status_Tapped(object sender, EventArgs e)
        {

            statuslbl.TextColor = Color.FromHex("#2f3e46");
            framestatus.BorderColor = Color.FromHex("#84a59d");
            chartView.IsVisible = true;

            dashlbl.TextColor = Color.FromHex("#84a59d");
            framedash.BorderColor = Color.White;
            frameView.IsVisible = false;
        }
        public void onDashboardAppearance()
        {
            dashlbl.TextColor = Color.FromHex("#2f3e46");
            framedash.BorderColor = Color.FromHex("#84a59d");
            frameView.IsVisible = true;

            statuslbl.TextColor = Color.FromHex("#84a59d");
            framestatus.BorderColor = Color.White;
            chartView.IsVisible = false;
        }

        async Task DashboardFigure()
        {
            // get all users in the system
            var allUsers = await dataUser.GetUsersAsync();
            if (allUsers.Count() > 0)
                totaluser.Text = allUsers.Count().ToString();
            else if (allUsers.Count() == 0)
                totaluser.Text = "0";

            // get only blocked users
            var blockedUsers = allUsers.Where(U => U.Blocked).ToList();
            if (blockedUsers.Count() > 0)
                userblocked.Text = blockedUsers.Count().ToString();
            else userblocked.Text = "0";

            // get all lands created in the system
            var allLands = await dataLand.GetlandsAsync();
            if(allLands.Count() > 0)
                landsites.Text = allLands.Count().ToString();
            else landsites.Text = "0";
           
            // get all userland
            var alluserLand = await dataUserLand.GetUserlandsAsync();
            // get only userlands whose requested is true
            var requestedUserLands = alluserLand.Where(ul => ul.Requested).ToList();
            if(requestedUserLands.Count() > 0)
                Viewingrequest.Text = requestedUserLands.Count().ToString();
            else Viewingrequest.Text = "0";
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