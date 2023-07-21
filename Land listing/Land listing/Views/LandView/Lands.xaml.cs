using Land_listing.Models;
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
        Models.User getUser;
		public Lands (User user)
		{
			InitializeComponent ();
            BindingContext = new LandViewModel();
            getUser = user;
            if (user.Usertype == "Administrator")
            {
                
            }
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
    }
}