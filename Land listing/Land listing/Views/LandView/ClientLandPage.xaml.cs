using Land_listing.Models;
using Land_listing.Services;
using Land_listing.ViewModels.Land;
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
    public partial class ClientLandPage : ContentPage
    {
        public IToast Datatoast { get; }
        Models.User GetUser;
        public ClientLandPage(User user)
        {
            InitializeComponent();
            BindingContext = new LandViewModel();
            Datatoast= DependencyService.Get<IToast>();
            GetUser = user;
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is LandViewModel viewModel)
            {
                 viewModel.userId = GetUser.UserId;
                await viewModel.refreshCommand.ExecuteAsync();
            }
        }
        private async void userRequest_Clicked(object sender, EventArgs e)
        {
            var result = await App.Current.MainPage.DisplayAlert("Alert", "Do you want to send a viewing request for this land?", "Yes", "No");
            if (result)
            {
                ImageButton button = (ImageButton)sender;
                var land = (Land)button.CommandParameter;
                if (BindingContext is LandViewModel viewModel)
                {
                    await viewModel.processLandViewingCommand.ExecuteAsync(land);
                   
                }
            }
            else
                return;
        }
        private async void NotificationAlert_Clicked(object sender, EventArgs e)
        {
            // go to notification page
            await Shell.Current.Navigation.PushAsync(new Notifications(GetUser));
        }

        private async void EditProfile_Clicked(object sender, EventArgs e)
        {
            // go to editUser page
            await Shell.Current.Navigation.PushAsync(new EditUserPage(GetUser));
        }
    }
}