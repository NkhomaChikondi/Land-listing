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
	public partial class SignUpView : ContentPage
	{
		public SignUpView ()
		{
			InitializeComponent ();
		}

        private void client_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (client.IsChecked)
                administrator.IsChecked = false;
        }

        private void administrator_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (administrator.IsChecked)
                client.IsChecked = false;
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await Shell.Current.Navigation.PushAsync(new SignInuser());
        }
    }
}