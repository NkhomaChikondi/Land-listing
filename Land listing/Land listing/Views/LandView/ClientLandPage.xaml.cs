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
    public partial class ClientLandPage : ContentPage
    {
        public IToast Datatoast { get; }
        public ClientLandPage(User user)
        {
            InitializeComponent();
            BindingContext = new LandViewModel();
            Datatoast= DependencyService.Get<IToast>();
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is LandViewModel viewModel)
            {
                await viewModel.refreshCommand.ExecuteAsync();
            }
        }
        private async void userRequest_Clicked(object sender, EventArgs e)
        {
            var result = await App.Current.MainPage.DisplayAlert("Alert", "Do you want to send a viewing request for this land?", "Yes", "No");
            if(result)
            {
                Datatoast.toast("Your Land viewwing request has been sent");
                return;
            }
            ImageButton button = (ImageButton)sender;
            var land = (Land)button.CommandParameter;

        }

      
    }

}