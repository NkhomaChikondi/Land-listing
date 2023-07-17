using Land_listing.Models;
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
    public partial class ClientLandPage : ContentPage
    {
        public ClientLandPage(User user)
        {
            InitializeComponent();
            BindingContext = new LandViewModel();
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is LandViewModel viewModel)
            {
                await viewModel.refreshCommand.ExecuteAsync();
            }
        }
        private void userRequest_Clicked(object sender, EventArgs e)
        {
            ImageButton button = (ImageButton)sender;
            var land = (Land)button.CommandParameter;
        }

      
    }

}