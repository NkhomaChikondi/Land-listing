using MvvmHelpers.Commands;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Land_listing.Models;

namespace Land_listing.ViewModels.User
{
    public class UserViewModel: BaseViewModel
    {
        //commands
        public AsyncCommand deleteUserCommand { get; set; }
        public AsyncCommand<int> UpdateUserCommand { get; set; }
        public AsyncCommand<Models.User> BlockUsercommand { get; set; }
        public AsyncCommand refreshCommand { get; set; }

        // collections
        public ObservableRangeCollection<Models.User> Users { get; }
        public UserViewModel()
        {
            deleteUserCommand = new AsyncCommand(deleteUser);
            UpdateUserCommand = new AsyncCommand<int>(updateUser);
            BlockUsercommand = new AsyncCommand<Models.User>(blockUser);
            refreshCommand = new AsyncCommand(Refresh);
            Users = new ObservableRangeCollection<Models.User>();
        }

        private async Task blockUser(Models.User user)
        {
            // check if the user has already been blocked
            if(user.Blocked)
            {
                await App.Current.MainPage.DisplayAlert("Alert", "User already blocked", "Ok");
                return;
            }
            else if(!user.Blocked)
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
            await dataUser.DeleteUserAsync(id);
            await refreshCommand.ExecuteAsync();
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
            // retrieve the users back
            Users.AddRange(users);
            // set "isBusy" to true
            IsBusy = false;
        }
    }
}
