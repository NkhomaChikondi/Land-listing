using Land_listing.Models;
using Land_listing.Views.LandView;
using Land_listing.Views.UserView;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Land_listing.ViewModels.Land
{
    public class LandViewModel: BaseViewModel
    {
        //commands
        public AsyncCommand deleteLandCommand { get; set; }
        public AsyncCommand addlandCommand { get; set; }
        public AsyncCommand<int> UpdateLandCommand { get; set; }       
        public AsyncCommand refreshCommand { get; set; }
        //collections
        public ObservableRangeCollection<Models.Land> Lands { get; }
        public LandViewModel()
        {
            deleteLandCommand = new AsyncCommand(deleteUser);
            UpdateLandCommand = new AsyncCommand<int>(updateUser);
            addlandCommand = new AsyncCommand(addland);
            refreshCommand = new AsyncCommand(Refresh);
            Lands = new ObservableRangeCollection<Models.Land>();
        }

        private async Task addland()
        {
            // navigate to add land page
            await Shell.Current.Navigation.PushAsync(new AddlandPage());
        }

        private Task deleteUser()
        {
            throw new NotImplementedException();
        }

        private Task updateUser(int arg)
        {
            throw new NotImplementedException();
        }

        private async Task Refresh()
        {           
            IsBusy = true;
            // clear users on the page
            Lands.Clear();
            // get all users
            var lands = await dataLand.GetlandsAsync();
            // retrieve the users back
            Lands.AddRange(lands);
            // set "isBusy" to true
            IsBusy = false;
        }
    }
    
}
