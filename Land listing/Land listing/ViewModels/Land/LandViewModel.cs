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
        List<userlanddata> userlanddatas = new List<userlanddata>(); 
        public int landId;
        private string landName;

        public string LandName { get => landName; set => landName = value; }
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
        public ObservableRangeCollection<Notification> notifications { get; }
       

        public LandViewModel()
        {
            deleteLandCommand = new AsyncCommand(deleteUser);
            UpdateLandCommand = new AsyncCommand<int>(updateUser);
            addlandCommand = new AsyncCommand(addland);
            refreshCommand = new AsyncCommand(Refresh);
            LoadUserLandDataCommand = new AsyncCommand (LoadUserLandData);
            processLandViewingCommand = new AsyncCommand<Models.Land>(processLandViewing);
            Lands = new ObservableRangeCollection<Models.Land>();
            User_Land = new ObservableRangeCollection<userlanddata>();
            notifications = new ObservableRangeCollection<Notification>();
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
        public async Task AddNotification(string landname, int userId)
        {
            try
            {
                if (IsBusy)
                    return;
                else
                {
                    // create a new notification
                    var newNotification = new Notification
                    {
                        Title = "Site viewing approval",
                        Message = $"Your site {landname} viewing request, has been approved!",
                        userId = userId,
                        Created = DateTime.Now
                    };
                    // add to the database
                    await dataNotification.AddNotificationAsync(newNotification);
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        private async Task LoadUserLandData()
        {
            IsBusy = true;
            // clear users on the page
            User_Land.Clear();        
            // get all userland
            var user_lands = await dataUserLand.GetUserlandsAsync ();
            // get only records having the land Id
            var landrecords = user_lands.Where(l => l.landId == landId).ToList();           
            // loop throughthe landrecords
            foreach (var land in landrecords)
            {
                // check if the user has requested land viewing
                // get all user_lands having the user id
                var alluserLands = user_lands.Where(ul => ul.UserId == land.UserId && ul.landId == landId).ToList();
                if (alluserLands.Count() > 0)
                {
                    // loop through the userlands and create new objects
                    foreach (var item in alluserLands)
                    {
                        // get the land having the item.landId
                        var user = await dataUser.GetUserAsync(item.UserId);
                        if (user != null)
                        {
                            // create a new userlanddata object
                            var newuserlanddata = new userlanddata
                            {
                                Id = item.UserId,
                                FullName = user.FullName,  
                                UserName = user.Username
                            };
                            userlanddatas.Add(newuserlanddata);
                        }
                    }
                }
            }
            // Check if userlanddatas contains any data before adding it to User_Land
            if (userlanddatas.Count() > 0)
            {
                User_Land.AddRange(userlanddatas);
            }
            // retrieve the users back
            //User_Land.AddRange(UserLands);
            // set "isBusy" to true
            IsBusy = false;
        }
        public async Task LoadNotifications(Models.User user)
        {
            IsBusy = true;
            // clear users on the page
            notifications.Clear();
            // get all users
            var dbnotifications = await dataNotification.GetNotificationsAsync();
            // take only notifications that has the user id
            var userNotifications = dbnotifications.Where( n => n.userId == user.UserId ).ToList(); 
            // retrieve the users back
            notifications.AddRange(userNotifications);
            // set "isBusy" to true
            IsBusy = false;
        }
    }
    
}
