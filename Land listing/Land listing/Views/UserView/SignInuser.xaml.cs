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
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await Shell.Current.Navigation.PushAsync(new SignUpView());
        }
    }
}