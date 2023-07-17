﻿using Land_listing.Models;
using Land_listing.ViewModels.Land;
using Land_listing.ViewModels.User;
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
	public partial class Lands : ContentPage
	{
		public Lands (User user)
		{
			InitializeComponent ();
            BindingContext = new LandViewModel();
            if (user.Usertype == "Administrator")
            {
                
            }
        }
        public Lands()
        {
            InitializeComponent();
            BindingContext = new LandViewModel();
            //if (user.Usertype == "Administrator") 
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            
            if (BindingContext is LandViewModel viewModel)
            {
                await viewModel.refreshCommand.ExecuteAsync();
            }            
        }
    }
}