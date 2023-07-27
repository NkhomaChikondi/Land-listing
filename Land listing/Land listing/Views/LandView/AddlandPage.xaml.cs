using Land_listing.Services;
using Land_listing.ViewModels.Land;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Land_listing.Views.LandView
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddlandPage : ContentPage
	{
        public IToast Datatoast { get; }
        public AddlandPage ()
		{
			InitializeComponent ();
            Datatoast = DependencyService.Get<IToast> ();
		}

        private async void FromGallery_Clicked(object sender, EventArgs e)
        {
            var Mediaimage = await MediaPicker.PickPhotoAsync( new MediaPickerOptions
            {
                Title = "Select photo"                            
            });
            var mediaResult = Mediaimage.FullPath;
            if(Mediaimage != null)
            {
                if (BindingContext is addlandViewModel viewModel)
                {
                    viewModel.Imagepath = mediaResult;
                    //await viewModel.AddLandCommand.ExecuteAsync();
                    Datatoast.toast("Picture selected");
                    //imgFrame.IsVisible = true;
                    return;
                }
            }
            else
            {
                Datatoast.toast("Cancelled");
                //imgFrame.IsVisible = true;
            }            
        }
        private async void TakePhoto_Clicked (object sender, EventArgs e)
        {
            var Mediaimage = await MediaPicker.CapturePhotoAsync(new MediaPickerOptions
            {
                Title = "Select photo"
            });
            var mediaResult = Mediaimage.FullPath;
            
            if(Mediaimage != null)
            {
                if (BindingContext is addlandViewModel viewModel)
                {
                    viewModel.Imagepath = mediaResult;
                    //await viewModel.AddLandCommand.ExecuteAsync();
                    Datatoast.toast("Picture taken");
                    //imgFrame.IsVisible = true;
                    //var stream = await Mediaimage.OpenReadAsync();
                    //resultImage.Source = ImageSource.FromStream(() => stream);
                    return;                    
                }
            }
            else
            {
                //imgFrame.IsVisible = false;
                Datatoast.toast("Cancelled");
            }             

        }
    }
}