using Land_listing.Models;
using Land_listing.Views.LandView;
using Land_listing.Views.UserView;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Land_listing.ViewModels.Land
{
    public class addlandViewModel: BaseViewModel
    {
        private string name;
        private string location;
        private string description;
        private string plotsize;
        private double price;
        private int landId;
        private int num1;
        private int num2;
        private string imagepath;
        
        //commands
        public AsyncCommand AddLandCommand { get; set; }
        public AsyncCommand deleteLandCommand { get; set; }
        public AsyncCommand UpdateLandCommand { get; set; }


        public string Name { get => name; set => name = value; }
        public string Location { get => location; set => location = value; }
        public string Plotsize { get => plotsize; set => plotsize = value; }
        public double Price { get => price; set => price = value; }
        public int LandId { get => LandId1; set => LandId1 = value; }
        public int LandId1 { get => landId; set => landId = value; }
        public int Num1 { get => num1; set => num1 = value; }
        public int Num2 { get => num2; set => num2 = value; }
        public string Imagepath { get => imagepath; set => imagepath = value; }
        public string Description { get => description; set => description = value; }

        public addlandViewModel()
        {
            AddLandCommand = new AsyncCommand(addLand);
            deleteLandCommand = new AsyncCommand(deleteLand);
            UpdateLandCommand = new AsyncCommand(updateLand);
        }

        private async Task updateLand()
        {

            // get all user in the database
            var alllands = await dataLand.GetlandsAsync();
            // check if their is any field having the userId
            var updatedLand = alllands.Where(u => u.LandId == LandId).FirstOrDefault();
            if (updatedLand != null)
            {
                name = char.ToUpper(name[0]) + name.Substring(1);                     
              
                // create a new user object
                var newLandObject = new Models.Land
                {
                    LandId = LandId,
                    LandName = name,
                    Location = location,
                    PlotSize = plotsize,
                    Price = price                    
                };
                await dataLand.UpdatelandAsync(newLandObject);
                // go to this page                
                await Shell.Current.Navigation.PushAsync(new Lands());
                Datatoast.toast("Land details updated successfully");
                
            }
        }
        private async Task deleteLand()
        {
            var result = await App.Current.MainPage.DisplayAlert("Alert", "By deleting, all viewing requests associated with it, will be deleted too. Continue?", "Yes", "No");
            if (result)
            {
                // get all userlands
                var userlands = await dataUserLand.GetUserlandsAsync();
                // get only those having the same userid as the incoming one
                var useralandId = userlands.Where(u => u.landId == LandId1).ToList();
                if (useralandId.Count > 0)
                {
                    // delete all of them
                    foreach (var item in useralandId)
                    {
                        await dataUserLand.DeleteUserlandAsync(item.Id);
                    }
                }
                await dataLand.DeletelandAsync(LandId1);
                // navigate to login page
                await Shell.Current.Navigation.PushAsync(new Lands());
            }
        }

        private async Task addLand()
        {
              if (IsBusy)
                return;
            try
            {
             
                // make sure land name is not empty
                if( string.IsNullOrEmpty(name))
                {
                    await App.Current.MainPage.DisplayAlert("Error", " Please enter the lands name", "Ok");
                    return;
                }
                else if (string.IsNullOrEmpty(Location))
                {
                    await App.Current.MainPage.DisplayAlert("Error", " Please enter land's location", "Ok");
                    return;
                }
                else if (string.IsNullOrEmpty(Description))
                {
                    await App.Current.MainPage.DisplayAlert("Error", " Please enter land's description", "Ok");
                    return;
                }
                else if (price == 0)
                {
                    await App.Current.MainPage.DisplayAlert("Error", " Please enter land's price", "Ok");
                    return;
                }
                else if (num1 == 0 || num2 == 0)
                {
                    await App.Current.MainPage.DisplayAlert("Error", " Please enter land's Plot size", "Ok");
                    return;
                }
                // make the first letter of lands name to be in big letters
                var UppercasedLandName = char.ToUpper(name[0]) + name.Substring(1);
                // get all lands from the database
                var lands = await dataLand.GetlandsAsync();
                // get the land in the database having a name similar to the uppercased name
                var dbland = lands.Where(l => l.LandName == UppercasedLandName).FirstOrDefault();
                if (dbland != null)
                {
                    // check if it has the same location two
                    if (dbland.Location == location)
                    {
                        await App.Current.MainPage.DisplayAlert("Error", "The land having the same name and location exist, change it", "Ok");
                        return;
                    }
                }
                plotsize = $"{num1} X {num2}";
                // create a new land object
                var newLand = new Models.Land
                {
                    LandName = UppercasedLandName,
                    Location = location,
                    PlotSize = plotsize,
                    Price = price,
                    Description = description,
                    MyimagePath = imagepath,
                    CreatedOn = DateTime.Now,
                };

                // add the new object to the database
                await dataLand.AddlandAsync(newLand);
                // navigate to lands page
                await Shell.Current.Navigation.PushAsync(new Lands());
                Datatoast.toast("New landsite added successfully");
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
