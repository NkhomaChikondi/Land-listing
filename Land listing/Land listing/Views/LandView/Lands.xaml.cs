using Land_listing.Models;
using Land_listing.Services;
using Land_listing.ViewModels.Land;
using Land_listing.ViewModels.User;
using Land_listing.Views.UserView;
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
	public partial class Lands : ContentPage
	{
        public IdataLand<Models.Land> dataLand { get; set; }
        Models.User getUser;
		public Lands (User user)
		{
			InitializeComponent ();
            dataLand = DependencyService.Get<IdataLand<Models.Land>>();
            BindingContext = new LandViewModel();
            getUser = user;          
            
        }
        public Lands()
        {
            InitializeComponent();
            BindingContext = new LandViewModel();
            //if (user.Usertype == "Administrator") 
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            // get all lands in the database
            var lands = await dataLand.GetlandsAsync();
            if(lands.Count() == 0)
            {
                stacklist.IsVisible = false;
                stackname.IsVisible = true;
                stackheader.IsVisible = false;
                stackimage.IsVisible = false;
            }
            else if(lands.Count() > 0)
            {
                stacklist.IsVisible = true;
                stackname.IsVisible = false;
                stackheader.IsVisible = true;
                stackimage.IsVisible = true;
            }
            if (BindingContext is LandViewModel viewModel)
            {
                await viewModel.refreshCommand.ExecuteAsync();
            }            
        }

        private async void Delete_Clicked(object sender, EventArgs e)
        {
            ImageButton button = (ImageButton)sender;
            var land = (Land)button.CommandParameter;
            // navigate to this page
            await Shell.Current.Navigation.PushAsync(new EditLandPage(land));
        }

        private async void UserRequest_Clicked(object sender, EventArgs e)
        {
            ImageButton button = (ImageButton)sender;
            var land = (Land)button.CommandParameter;
            // navigate to this page
            await Shell.Current.Navigation.PushAsync(new UserlandView(land.LandId,getUser));
        }

        private void sendMoney_Clicked(object sender, EventArgs e)
        {

        }

        private async void ImageButton_Clicked(object sender, EventArgs e)
        {
            var action = await DisplayActionSheet("Sort by:", "Cancel", "", "All", "Location", "Price", "Date Created");
            if (action == "All")
            {
                if (BindingContext is LandViewModel viewModel)
                {
                    viewModel.AllLands();
                }
            }
            if (action == "Location")
            {
                if (BindingContext is LandViewModel viewModel)
                {
                    viewModel.landLocation();
                }
            }
            if (action == "Price")
            {
                if (BindingContext is LandViewModel viewModel)
                {
                    viewModel.landPrice();
                }
            }
            if (action == "Date Created")
            {
                if (BindingContext is LandViewModel viewModel)
                {
                    viewModel.dateCreated();
                }
            }
        }
    }
}