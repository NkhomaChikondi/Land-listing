﻿using Land_listing.Models;
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
        private bool location;
        private bool price;
        private bool all = true;
        private bool date;

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
        public bool Location { get => location; set => location = value; }
        public bool Price { get => price; set => price = value; }
        public bool All { get => all; set => all = value; }
        public bool Date { get => date; set => date = value; }

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

        public void AllLands()
        {
            location = false;
            price = false;
            date = false;
            All = true;
            Refresh();
        }
        public void landLocation()
        {
            location = true;
            price = false;
            date = false;
            All = false;
            Refresh();
        }
        public void landPrice()
        {
            location = false;
            price = true;
            date = false;
            All = false;
            Refresh();
        }
        public void dateCreated()
        {
            location = false;
            price = false;
            date = true;
            All = false;
            Refresh();
        }
        private async Task Refresh()
        {           
            IsBusy = true;
            // clear users on the page
            Lands.Clear();
            // get all users
            var lands = await dataLand.GetlandsAsync();
            if(all)
            // retrieve the users back
            Lands.AddRange(lands);
            else if(location)
            {
                var locationland = lands.OrderBy(L => L.Location).ToList();
                Lands.AddRange(locationland);
            }
            else if(price)
            {
                var priceland = lands.OrderBy(L => L.Price).ToList();
                Lands.AddRange(priceland);
            }
            else if(date)
            {
                var dateland = lands.OrderBy(L => L.CreatedOn).ToList();
                Lands.AddRange(dateland);
            }
            // set "isBusy" to true
            IsBusy = false;
        }
        public async Task AddNotification(string landname, int userId,int landId)
        {
            // get all user lands in the database
            var allUserlands = await dataUserLand.GetUserlandsAsync();
            // get only the record having the incoming userid and land id
            var userland = allUserlands.Where(ul => ul.UserId == userId && ul.landId == landId).FirstOrDefault();
            if(userland != null) 
            {
                // check if requested is true
                if (userland.Requested)
                    await App.Current.MainPage.DisplayAlert("Alert", "The viewing request of this land, has already been approved", "Ok");
                else 
                {
                    // update userland
                    userland.Requested = true;
                    await dataUserLand.UpdateUserlandAsync(userland);

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
                            await LoadUserLandData();
                            Datatoast.toast("Approved notification sent");
                        }
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
               
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
                    userlanddatas.Clear();
                    // loop through the userlands and create new objects
                    foreach (var item in alluserLands)
                    {
                        string approve;
                        // get the land having the item.landId
                        var user = await dataUser.GetUserAsync(item.UserId);
                        if (user != null)
                        {
                            if (item.Requested)
                                approve = "Approved";
                            else approve = "Approve";
                            
                            // create a new userlanddata object
                            var newuserlanddata = new userlanddata
                            {
                                Id = item.UserId,
                                FullName = user.FullName,  
                                UserName = user.Username,
                                Approve = approve
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
