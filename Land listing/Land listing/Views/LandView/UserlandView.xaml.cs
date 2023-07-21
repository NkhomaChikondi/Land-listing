using Land_listing.Models;
using Land_listing.Services;
using Land_listing.ViewModels.Land;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Land_listing.Views.LandView
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserlandView : ContentPage
    {
        public IToast Datatoast { get; }
        public IdataLand<Models.Land> dataLand { get; set; }
        public IdataUserland<Models.User_Land> dataUserLand { get; set; }
        int getLandId;
        User getUser;
        string sitename;
        public UserlandView( int id,User user)
        {
            InitializeComponent();
            BindingContext = new LandViewModel();
            dataLand = DependencyService.Get<IdataLand<Models.Land>>();
            dataUserLand = DependencyService.Get<IdataUserland<User_Land>>();
            Datatoast = DependencyService.Get<IToast>();
            getLandId = id;
            getUser = user;
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            // get land name having the id
            var landName = await dataLand.GetlandAsync(getLandId);
            sitename = landName.LandName;
            landname.Text = landName.LandName;
            if (BindingContext is LandViewModel viewModel)
            {
                viewModel.landId = getLandId;
                await viewModel.LoadUserLandDataCommand.ExecuteAsync();
            }
            

        }
        private async void Approve_Clicked(object sender, EventArgs e)
        {

            Button button = (Button)sender;
            var Userland = (userlanddata)button.CommandParameter;
            if (BindingContext is LandViewModel viewModel)
            {
                await viewModel.AddNotification(sitename, Userland.Id,getLandId);
            }
                      
        }
    }
}