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
using static Land_listing.ViewModels.Land.LandViewModel;

namespace Land_listing.ViewModels.Land
{
    public class LandViewModel: BaseViewModel
    {
        public int userId { get; set; }
        List<userlanddata> userlanddatas ;
        //commands
        public AsyncCommand deleteLandCommand { get; set; }
        public AsyncCommand addlandCommand { get; set; }
        public AsyncCommand<int> UpdateLandCommand { get; set; }       
        public AsyncCommand refreshCommand { get; set; }
        public AsyncCommand<Models.Land> processLandViewingCommand { get; set; }
        public AsyncCommand LoadUserLandDataCommand { get; set; }
        //collections
        public ObservableRangeCollection<Models.Land> Lands { get; }
        public ObservableRangeCollection<userlanddata> User_Land { get; }
        public LandViewModel()
        {
            deleteLandCommand = new AsyncCommand(deleteUser);
            UpdateLandCommand = new AsyncCommand<int>(updateUser);
            addlandCommand = new AsyncCommand(addland);
            refreshCommand = new AsyncCommand(Refresh);
            LoadUserLandDataCommand = new AsyncCommand(LoadUserLandData);
            processLandViewingCommand = new AsyncCommand<Models.Land>(processLandViewing);
            Lands = new ObservableRangeCollection<Models.Land>();
            User_Land = new ObservableRangeCollection<userlanddata>();
        }

        private async Task processLandViewing(Models.Land land)
        {
            // make sure both land and userId are not empty
            if (land != null && userId != 0)
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
                else 
                {
                    // create a new user land object
                    var newUserland = new Models.User_Land
                    {
                        UserId = userId,
                        landId = land.LandId,
                        Requested = true
                    };
                    // add to the database
                    await dataUserLand.AddUserlandAsync(newUserland);
                    Datatoast.toast("Your Land viewing request has been sent");
                    return;
                }
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
        
        private async Task LoadUserLandData()
        {
            IsBusy = true;
            // clear users on the page
            User_Land.Clear();
            // get all lands
            var lands = await dataLand.GetlandsAsync();
            // get all users
            var users = await dataUser.GetUsersAsync();
            // get all userland
            var user_lands = await dataUserLand.GetUserlandsAsync ();
           
            foreach ( var user in users )
            {
              // check if the user has requested land viewing
              // get all user_ lands having the user id
              var alluserLands = user_lands.Where(ul => ul.UserId == user.UserId).ToList();
                if (alluserLands.Count() > 0)
                {
                    // loop through the userlands and create new object
                    foreach (var item in alluserLands)
                    {
                        // get the land having the item.landId
                        var itemLand = await dataLand.GetlandAsync(item.landId);
                        if (itemLand != null)
                        {
                            // create a new list object
                            var newuserlanddatas = new userlanddata
                            {
                                Id = item.UserId,
                                FullName = user.Username,
                                LandName = itemLand.LandName
                            };
                            userlanddatas.Add(newuserlanddatas);
                        }                      
                    }
                }               
            }
            if(userlanddatas.Count() == 0)
            {
                return;
            }
            else
            User_Land.AddRange(userlanddatas);
            // retrieve the users back
            //User_Land.AddRange(UserLands);
            // set "isBusy" to true
            IsBusy = false;
        }
        public class userlanddata
        {
            public int Id { get; set; }
            public string FullName { get; set; }
            public string LandName { get; set; }
        }
    }
    
}
