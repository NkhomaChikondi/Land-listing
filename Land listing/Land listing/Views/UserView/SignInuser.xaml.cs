using Land_listing.ViewModels.User;
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
    public partial class SignInuser : ContentPage
    {
        public SignInuser()
        {
            InitializeComponent();
            BindingContext = new AddUserViewModel();
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            if(BindingContext is AddUserViewModel viewModel)
            {
                await viewModel.createUser();
            }
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await Shell.Current.Navigation.PushAsync(new SignUpView());
        }
    }
}