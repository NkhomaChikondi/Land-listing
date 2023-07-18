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
        public UserlandView()
        {
            InitializeComponent();
            BindingContext = new LandViewModel();
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is LandViewModel viewModel)
            {
                //viewModel.userId = GetUser.UserId;
                await viewModel.LoadUserLandDataCommand.ExecuteAsync();
            }
        }
    }
}