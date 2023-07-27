using Land_listing.Models;
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
    public partial class viewImage : ContentPage
    {
        public viewImage( Land land)
        {
            InitializeComponent();
            landImage.Source = land.MyimagePath;
        }
    }
}