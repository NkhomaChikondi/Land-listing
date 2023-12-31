﻿using Land_listing.Models;
using Land_listing.Services;
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
	public partial class userPage : ContentPage
	{
        public IDataUser<Models.User> dataUser { get; set; }
        public userPage ()
		{
			InitializeComponent ();
            BindingContext = new UserViewModel();
            dataUser = DependencyService.Get<IDataUser<Models.User>>();   

        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            // get all users in the system
            var users = await dataUser.GetUsersAsync ();
            if(users.Count() == 0)
            {
                stackUserlist.IsVisible = false;
                stackuserEmpty.IsVisible = true;
            }
            else
            {
                stackUserlist.IsVisible = true;
                stackuserEmpty.IsVisible = false;
            }
            if(BindingContext is  UserViewModel viewModel)
            {
               await viewModel.Refresh();
            }
        }
        private async void UpdateUser_Clicked(object sender, EventArgs e)
        {
            ImageButton button = (ImageButton)sender;            
            var user = (User)button.CommandParameter;
            // navigate to details page
            await Shell.Current.Navigation.PushAsync(new userDetails(user));
        }
        private async void Deleteuser_Clicked(object sender, EventArgs e)
        {
            ImageButton button = (ImageButton)sender;
            var user = (User)button.CommandParameter;
            var result = await App.Current.MainPage.DisplayAlert("Alert", "This User will be deleted, Continue?", "Yes", "Cancel");
            if (result)
            {
                if (BindingContext is UserViewModel viewModel)
                {
                    // call the deleting command
                    await viewModel.UpdateUserCommand.ExecuteAsync(user.UserId);                  
                }
            }
            else
                return;
        }
        private async void BlockUser_Clicked(object sender, EventArgs e)
        {
            ImageButton button = (ImageButton)sender;
            var user = (User)button.CommandParameter;
            if (BindingContext is UserViewModel viewModel)
            {
                // call the deleting command
                await viewModel.BlockUsercommand.ExecuteAsync(user);
               // await App.Current.MainPage.DisplayAlert("Alert", "User blocked successfully", "Ok");
            }
        }
        private  async void ImageButton_Clicked(object sender, EventArgs e)
        {
            var action = await DisplayActionSheet("Sort by:", "Cancel", "","All", "Ascending", "Descending","Date Created");
            if (action == "All")
            {
                if (BindingContext is UserViewModel viewModel)
                {
                     viewModel.AllUsers();
                }
            }
            if (action == "Ascending")
            {
                if (BindingContext is UserViewModel viewModel)
                {
                    viewModel.AscendingUsers();
                }
            }
            if (action == "Descending")
            {
                if (BindingContext is UserViewModel viewModel)
                {
                    viewModel.DescendingUsers();
                }
            }
            if (action == "Date Created")
            {
                if (BindingContext is UserViewModel viewModel)
                {
                    viewModel.DateUser();
                }
            }
        }
    }
}