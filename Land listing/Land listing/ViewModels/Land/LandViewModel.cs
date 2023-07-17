using Land_listing.Models;
using Land_listing.Views.LandView;
using Land_listing.Views.UserView;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Land_listing.ViewModels.Land
{
    public class LandViewModel: BaseViewModel
    {
        public int userId { get; set; }
        //commands
        public AsyncCommand deleteLandCommand { get; set; }
        public AsyncCommand addlandCommand { get; set; }
        public AsyncCommand<int> UpdateLandCommand { get; set; }       
        public AsyncCommand refreshCommand { get; set; }
        public AsyncCommand<Models.Land> processLandViewingCommand { get; set; }
        //collections
        public ObservableRangeCollection<Models.Land> Lands { get; }
        public LandViewModel()
        {
            deleteLandCommand = new AsyncCommand(deleteUser);
            UpdateLandCommand = new AsyncCommand<int>(updateUser);
            addlandCommand = new AsyncCommand(addland);
            refreshCommand = new AsyncCommand(Refresh);
            processLandViewingCommand = new AsyncCommand<Models.Land>(processLandViewing);
            Lands = new ObservableRangeCollection<Models.Land>();
        }

        private async Task processLandViewing(Models.Land land)
        {
            // make sure both land and userId are not empty
            if (land != null && userId == 0)
            {
                // get all items in the user land
                var userlands = await dataUserLand.GetUserlandsAsync();
                // check if there is any record having the incoming userId and land Id
                var userland = userlands.Where(U => U.UserId == userId && U.landId == land.LandId).FirstOrDefault();
                if(userland != null)
                {
                    await App.Current.MainPage.DisplayAlert("Alert", "You have already sent a viewing request for this land", "Ok");
                    return;                
                }
                // create a new user land object
                var newUserland = new Models.User_Land
                {
                    UserId = userId,
                    landId = land.LandId,
                    Requested = true
                };
                // add to the database
                await dataUserLand.AddUserlandAsync(newUserland);
            }
            else return;
        }

        private async Task addland()
        {
            // navigate to add land page
            await Shell.Current.Navigation.PushAsync(new AddlandPage());
        }

        private async Task deleteUser()
        {
            
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
