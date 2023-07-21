using Land_listing.Models;
using Land_listing.Views.LandView;
using Land_listing.Views.UserView;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Land_listing.ViewModels.User
{
    public class AddUserViewModel: BaseViewModel, INotifyPropertyChanged
    {
        private string fullname;   
        private string username;
        private string password;
        private string phonenumber;
        private bool client = false;
        private bool administrator = false;
        private string confirmpassword;
        private string usertype;
        private int userId;

        public string Fullname { get => fullname; set => fullname = value; }
        public string Password { get => password; set => password = value; }
        public string Phonenumber { get => phonenumber; set => phonenumber = value; }
       

        //commands
        public AsyncCommand AddUserCommand { get; set; }
        public AsyncCommand UpdateUserCommand { get; set; }
        public AsyncCommand DeleteUserCommand { get; set; }
        public AsyncCommand LoginUserCommand { get; set; }

        // collections
        public ObservableRangeCollection<Models.User> Users { get; }
        public string Confirmpassword { get => Confirmpassword1; set => Confirmpassword1 = value; }
        public bool Client { get => client; set => client = value; }
        public string Confirmpassword1 { get => confirmpassword; set => confirmpassword = value; }
        public string Usertype { get => usertype; set => usertype = value; }
        public bool Administrator { get => administrator; set => administrator = value; }
        public string Username { get => username; set => username = value; }
        public int UserId { get => userId; set => userId = value; }

        public AddUserViewModel()
        {
            AddUserCommand = new AsyncCommand(AddUser);
            UpdateUserCommand = new AsyncCommand(updateUser);
            DeleteUserCommand = new AsyncCommand(deleteUser);
            LoginUserCommand = new AsyncCommand(LoginUser);
            Users = new ObservableRangeCollection<Models.User>();
        }

        private async Task deleteUser()
        {
            var result = await App.Current.MainPage.DisplayAlert("Alert", "You want to delete your account. Continue?", "Yes", "No");
            if (result)
            {
                // get all userlands
                var userlands = await dataUserLand.GetUserlandsAsync();
                // get only those having the same userid as the incoming one
                var useralandId = userlands.Where(u => u.UserId == userId).ToList();
                if (useralandId.Count > 0)
                {
                    // delete all of them
                    foreach (var item in useralandId)
                    {
                        await dataUserLand.DeleteUserlandAsync(item.Id);
                    }
                }
                await dataUser.DeleteUserAsync(userId);
                // navigate to login page
                await Shell.Current.Navigation.PushAsync(new SignInuser());
            }
            else
                return;

        }

        public async Task createUser()
        {
            // get all user in the database
            var allUsers = await dataUser.GetUsersAsync();
            // check if there is anyone with a username of admninstrator
            var administrator = allUsers.Where(U => U.Usertype == "Administrator").FirstOrDefault();
            if (administrator == null)
            {
                //create a new user having that name
                var newUser = new Models.User
                {
                    FullName = "Administrator",
                    Username = "Admin123",
                    PhoneNumber = "0998887776",
                    Password = "@Admin123",
                    Usertype = "Administrator",
                    Blocked = false,
                    CreatedOn = DateTime.Now,
                };
                // save to the database
                await dataUser.AddUserAsync(newUser);
            }
            else
                return;
        }
        private async Task AddUser()
        {
            // check if the app is busy
            if (IsBusy == true)
                return;
            try
            {
                // make sure the username doesnt exist in the database
                // get all users in the database
                var users = await dataUser.GetUsersAsync();
                // check if there is any user having the incoming user's username
                if(users.Any(U => U.Username == username))
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Username already exist", "Ok");
                    return;
                }
                if (users.Any(U => U.PhoneNumber == phonenumber))
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Phone number already exist", "Ok");
                    return;
                }               
               
                if (string.IsNullOrEmpty(fullname))
                {
                    await App.Current.MainPage.DisplayAlert("Error", " Please enter your first name", "Ok");
                    return;
                }
               
                // check if phone number is entered
                else if (string.IsNullOrEmpty(Phonenumber))
                {
                    await App.Current.MainPage.DisplayAlert("Error", " Please enter your Phone Number", "Ok");
                    return;
                }
                else if (string.IsNullOrEmpty(username))
                {
                    await App.Current.MainPage.DisplayAlert("Error", " Please enter your username", "Ok");
                    return;
                }
                else if (string.IsNullOrEmpty(password))
                {
                    await App.Current.MainPage.DisplayAlert("Error", " Please enter your Password", "Ok");
                    return;
                }

                // check if password and confirm password are similar
                if (!password.Equals(Confirmpassword1))
                {
                    await App.Current.MainPage.DisplayAlert("Error", " Make sure your password and confirm password are similar", "Ok");
                    return;
                }
                // call the password validator               
                if (PasswordValidator(password))
                {
                    // change firstname and last name to uppercase if they are not
                    // change the first letter of the Task name to upercase
                    var UppercasedfirstName = char.ToUpper(fullname[0]) + fullname.Substring(1);                   
                       
                    // create a new user
                    var newUser = new Models.User
                    {
                        FullName = UppercasedfirstName,
                        Username = Username,
                        CreatedOn = DateTime.Now,
                        PhoneNumber = phonenumber,
                        Password = password,
                        Blocked = false,
                        Usertype = "Client",
                    };

                    // add to the database
                    await dataUser.AddUserAsync(newUser);

                    // go to user page                
                    await Shell.Current.Navigation.PushAsync(new SignInuser());
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Make sure your password contains at least one letter, one special character, and one number", "Ok");
                    return;
                }
              
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to add new user: {ex.Message}");
                await App.Current.MainPage.DisplayAlert("Error!", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
        public async Task updateUser()
        {
            // get all user in the database
            var allUsers = await dataUser.GetUsersAsync();
            // check if their is any field having the userId
            var updatedUser = allUsers.Where(u => u.UserId == UserId).FirstOrDefault();
            if (updatedUser != null)
            {
                fullname = char.ToUpper(fullname[0]) + fullname.Substring(1);
                // call the password validator               
                if (PasswordValidator(password))
                {
                    // create a new user object
                    var newUserObject = new Models.User
                    {
                        UserId = UserId,
                        FullName = Fullname,
                        Password = Password,
                        Username = Username,
                        PhoneNumber = phonenumber,
                        CreatedOn= updatedUser.CreatedOn,
                        Usertype = updatedUser.Usertype,
                        Blocked = updatedUser.Blocked,
                    };
                    await dataUser.UpdateUserAsync(newUserObject);
                    // go to this page                
                    await Shell.Current.Navigation.PushAsync(new ClientLandPage(newUserObject));
                    Datatoast.toast("User details updated successfully");
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Make sure your password contains at least one letter, one special character, and one number", "Ok");
                    return;
                }
            }
        }
        static bool PasswordValidator(string password)
        {
            // Check if the password contains at least one letter, one special character, and one number
            Regex regex = new Regex(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@#$%^&+=])");
            return regex.IsMatch(password);            
        }
        async Task LoginUser()
        {
            // check if the app is busy
            if (IsBusy == true)
                return;
            try
            {
               // get all users in the database
                var users = await dataUser.GetUsersAsync();
                if (string.IsNullOrEmpty(username))
                {
                    await App.Current.MainPage.DisplayAlert("Error", " Please enter your username", "Ok");
                    return;
                }
                else if (string.IsNullOrEmpty(password))
                {
                    await App.Current.MainPage.DisplayAlert("Error", " Please enter your Password", "Ok");
                    return;
                }
                // make sure both the username and password are not empty

                // check if the username doesnt exist
                var dbUser = users.Where(t => t.Username == username).FirstOrDefault();
                if (dbUser != null)
                {
                    //check if the user is blocked or not
                    if(dbUser.Blocked)
                    {
                        await App.Current.MainPage.DisplayAlert("Alert", "Sorry, your account is blocked, you cant login", "Ok");
                        return;
                    }
                    else 
                    {
                        //// get the password having the incoming password
                        //var  dbPassword = users.Where(t => t.Password == password).FirstOrDefault();
                        // check if the password matches
                        if (dbUser.Password == password)
                        {
                            if (dbUser.Usertype.Equals("Administrator"))
                            {
                                // go to this page                
                                await Shell.Current.Navigation.PushAsync(new AdministratorDashboardView(dbUser));
                            }
                            else if (dbUser.Usertype.Equals("Client"))
                            {
                                // go to this page                
                                await Shell.Current.Navigation.PushAsync(new ClientLandPage(dbUser));
                            }
                        }
                        else
                        {
                            await App.Current.MainPage.DisplayAlert("Error", " Password doesnt exist", "Ok");
                            return;
                        }

                    }
                   
                }
                else 
                {
                    await App.Current.MainPage.DisplayAlert("Error", " username doesnt exist", "Ok");
                    return;
                }                    
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
