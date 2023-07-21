using Land_listing.Models;
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
	public partial class EditUserPage : ContentPage
	{
		User GetUser;
		public EditUserPage(User user)
		{
			InitializeComponent();
			BindingContext = new AddUserViewModel();
			GetUser = user;
			if(BindingContext is AddUserViewModel viewModel)
			{
                viewModel.Username = GetUser.Username;
				viewModel.Phonenumber = GetUser.PhoneNumber;
                viewModel.Fullname = GetUser.FullName;
                viewModel.Password = GetUser.Password;
                viewModel.Fullname = GetUser.FullName;
				viewModel.UserId = GetUser.UserId;


				 Fullname.Text = GetUser.FullName;
				Password.Text = GetUser.Password;
				Username.Text = GetUser.Username;
				PhoneNumber.Text = GetUser.PhoneNumber;
            }

        }
		protected async override void OnAppearing()
		{
			base.OnAppearing();
			if(BindingContext is AddUserViewModel viewModel)
			{
				
			}
		}

   //     private async void updateUser_Clicked(object sender, EventArgs e)
   //     {
			//// create a new user object
			//var newUserObject = new User
			//{
			//	UserId = GetUser.UserId,
			//	FullName = Fullname.Text,
			//	Password = Password.Text,
			//	Username= Username.Text,
			//	PhoneNumber = PhoneNumber.Text,				
			//};
			//if(BindingContext is AddUserViewModel viewModel)
			//{
			//	await viewModel.updateUser(newUserObject);
			//}
   //     }
    }
}