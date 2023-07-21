using Land_listing.Models;
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
	public partial class EditLandPage : ContentPage
	{
		public EditLandPage (Land land )
		{
			InitializeComponent ();
            BindingContext = new addlandViewModel();
            if (BindingContext is addlandViewModel viewModel)
            {
                viewModel.Name = land.LandName;
                viewModel.Location = land.Location;
                viewModel.Plotsize = land.PlotSize;
                viewModel.Price = land.Price;
                viewModel.LandId = land.LandId;

                name.Text = land.LandName;
                Location.Text = land.Location;
                Plotsize.Text = land.PlotSize;
                Price.Text = land.Price.ToString();                
            }
        }
	}
}