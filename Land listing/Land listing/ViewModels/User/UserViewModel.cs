using MvvmHelpers.Commands;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Land_listing.Models;
using System.Linq;

namespace Land_listing.ViewModels.User
{
    public class UserViewModel: BaseViewModel
    {
        private bool all = true;
        private bool ascending;
        private bool descending;
        private bool date;
        //commands
        public AsyncCommand deleteUserCommand { get; set; }
        public AsyncCommand<int> UpdateUserCommand { get; set; }
        public AsyncCommand<Models.User> BlockUsercommand { get; set; }
        public AsyncCommand refreshCommand { get; set; }

        // collections
        public ObservableRangeCollection<Models.User> Users { get; }
        public bool All { get => all; set => all = value; }
        public bool Ascending { get => ascending; set => ascending = value; }
        public bool Descending { get => descending; set => descending = value; }
        public bool Date { get => date; set => date = value; }

        public UserViewModel()
        {
            deleteUserCommand = new AsyncCommand(deleteUser);
            UpdateUserCommand = new AsyncCommand<int>(updateUser);
            BlockUsercommand = new AsyncCommand<Models.User>(blockUser);
            refreshCommand = new AsyncCommand(Refresh);
            Users = new ObservableRangeCollection<Models.User>();
        }
        public void AllUsers()
        {
            ascending = false;
            descending = false;
            date = false;
            All = true;
            Refresh();
        }
        public void AscendingUsers()
        {
            ascending = true;
            descending = false;
            date = false;
            All = false;
            Refresh();
        }
        public void DescendingUsers()
        {
            ascending = false;
            descending = true;
            date = false;
            All = false;
            Refresh();
        }
        public void DateUser()
        {
            ascending = false;
            descending = false;
            date = true;
            All = false;
            Refresh();
        }
        private async Task blockUser(Models.User user)
        {
            // check if the user has already been blocked
            if (user.Blocked)
            {
                await App.Current.MainPage.DisplayAlert("Alert", "User already blocked", "Ok");
                return;
            }
            else if (!user.Blocked)
            {
                var result = await App.Current.MainPage.DisplayAlert("Alert", "This User will be Blocked, Continue?", "Yes", "Cancel");
                if (result)
                {
                    user.Blocked = true;
                    // update user from the database
                    await dataUser.UpdateUserAsync(user);
                    await App.Current.MainPage.DisplayAlert("Alert", "User blocked successfully", "Ok");
                }


            }
        }
        private async Task updateUser(int id)
        {
            var result = await App.Current.MainPage.DisplayAlert("Alert", "All land viewing request (if any) associated with this account will be deleted", "Yes", "No");
            if (result)
            {
                // get all userlands
                var userlands = await dataUserLand.GetUserlandsAsync();
                // get only those having the same userid as the incoming one
                var useralandId = userlands.Where(u => u.UserId == id).ToList();
                if (useralandId.Count > 0)
                {
                    // delete all of them
                    foreach (var item in useralandId)
                    {
                        await dataUserLand.DeleteUserlandAsync(item.Id);
                    }
                }
                await dataUser.DeleteUserAsync(id);
                await refreshCommand.ExecuteAsync();
                Datatoast.toast("Account deleted successfully");
            }
            else
                return;
        }
        private Task deleteUser()
        {
            throw new NotImplementedException();
        }
        public async Task Refresh()
        {
            // set "IsBusy" to true
            IsBusy = true;
            // clear users on the page
            Users.Clear();
            // get all users
            var users = await dataUser.GetUsersAsync();
            //filter data
            if(all)
            { 
                // retrieve the users back
                Users.AddRange(users);
            }
            else if(ascending)
            { 
              var inAscending = users.OrderBy(U => U.FullName).ToList();
                Users.AddRange(inAscending);
            }
            else if(descending)
            {
                var indescending = users.OrderByDescending(U => U.FullName).ToList();
                Users.AddRange(indescending);
            }
            else if(Date)
            {
                var inDate = users.OrderBy(U => U.CreatedOn).ToList();
                Users.AddRange(inDate);
            }
            // set "isBusy" to true
            IsBusy = false;
        }
    }    
}
      


