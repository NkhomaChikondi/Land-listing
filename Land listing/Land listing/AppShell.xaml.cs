using Land_listing.Views.UserView;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Land_listing
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(SignUpView), typeof(SignUpView));
            Routing.RegisterRoute(nameof(AdministratorDashboardView), typeof(AdministratorDashboardView));
            Routing.RegisterRoute(nameof(userPage), typeof(userPage));
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.Navigation.PushAsync(new SignInuser());
        }
    }
}
