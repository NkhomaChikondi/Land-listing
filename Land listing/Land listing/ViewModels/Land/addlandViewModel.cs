using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Land_listing.ViewModels.Land
{
    public class addlandViewModel: BaseViewModel
    {
        private string name;
        private string location;
        private string plotsize;
        private double price;
        
        //commands
        public AsyncCommand AddLandCommand { get; set; }
        public string Name { get => name; set => name = value; }
        public string Location { get => location; set => location = value; }
        public string Plotsize { get => plotsize; set => plotsize = value; }
        public double Price { get => price; set => price = value; }

        public addlandViewModel()
        {
            AddLandCommand = new AsyncCommand(addLand);            
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
                else if (price == 0)
                {
                    await App.Current.MainPage.DisplayAlert("Error", " Please enter land's price", "Ok");
                    return;
                }
                else if (string.IsNullOrEmpty(plotsize))
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
                // create a new land object
                var newLand = new Models.Land
                {
                    LandName = UppercasedLandName,
                    Location = location,
                    PlotSize = plotsize,
                    Price = price,
                    CreatedOn = DateTime.Now,
                };

                // add the new object to the database
                await dataLand.AddlandAsync(newLand);
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {

            }
        }
    }
}
