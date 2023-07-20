using Land_listing.Models;
using Land_listing.ViewModels.Land;
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
    public partial class Notifications : ContentPage
    {
        User GetUser;
        public Notifications(User user)
        {
            InitializeComponent();
            BindingContext = new LandViewModel();
            GetUser = user;
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            if(BindingContext is LandViewModel viewModel)
            {
                await viewModel.LoadNotifications(GetUser);
            
            }
        }
    }
}