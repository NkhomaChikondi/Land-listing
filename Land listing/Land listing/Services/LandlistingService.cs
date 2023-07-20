using Land_listing.Models;
using Land_listing.Services;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(LandlistingService))]
namespace Land_listing.Services
{
    public class LandlistingService:IDataUser<User>,IdataLand<Land>,IdataUserland<User_Land>, INotification<Notification>
    {
        static SQLiteAsyncConnection db;
        // database connection class
        public static async Task Init()
        {
            if (db != null)
                return;
            //get path of the database file
            var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MyLandlistingData.db");

            db = new SQLiteAsyncConnection(databasePath);
            //  creating database tables
            await db.CreateTableAsync<User>();
            await db.CreateTableAsync<User_Land>();
            await db.CreateTableAsync<Land>();
            await db.CreateTableAsync<Notification>();
        }       

        public async Task<bool> AddUserAsync(User item)
        {
            await Init();
            // create a new user
            var user = new User
            {
                FullName = item.FullName,   
                Username = item.Username,
                Password = item.Password,
                PhoneNumber = item.PhoneNumber,               
                Usertype = item.Usertype,
                Blocked = item.Blocked,
                CreatedOn = item.CreatedOn,
            };
            // insert the values into the database
            await db.InsertAsync(user);
            return await Task.FromResult(true);
        }      
        public async Task<User> GetUserAsync(int id)
        {
            await Init();
            // get the selected item 
            var userId = await db.Table<User>().Where(d => d.UserId == id).FirstOrDefaultAsync();
            return userId;
        }
        public async Task<IEnumerable<User>> GetUsersAsync(bool forceRefresh = false)
        {
            await Init();
            // get all the users in the database
            var allUsers = await db.Table<User>().ToListAsync();
            return allUsers;
        }
        public async Task<bool> UpdateUserAsync(User item)
        {
            // modifying users item in the database
            await Init();
            var updateUsers = await db.UpdateAsync(item);
            return await Task.FromResult(true);
        }
        public async Task<bool> DeleteUserAsync(int id)
        {
            await Init();
            // Remove the selected category item from the database
            var deleteUser = await db.DeleteAsync<User>(id);
            return await Task.FromResult(true);
        }


        public async Task<bool> AddlandAsync(Land item)
        {
            await Init();
            // create a new land
            var land = new Land
            {
                LandName = item.LandName,
                PlotSize = item.PlotSize,               
                Location = item.Location,
                Price = item.Price,
                CreatedOn = item.CreatedOn,
            };
            // add to the database
            await db.InsertAsync(land);
            return await Task.FromResult(true);
        }
        public async Task<Land> GetlandAsync(int id)
        {
            await Init();
            // get the selected item 
            var landid = await db.Table<Land>().Where(d => d.LandId == id).FirstOrDefaultAsync();
            return landid;
        }
        public async Task<IEnumerable<Land>> GetlandsAsync(bool forceRefresh = false)
        {
            await Init();
            // get all the users in the database
            var alllands = await db.Table<Land>().ToListAsync();
            return alllands;
        }
        public async Task<bool> DeletelandAsync(int id)
        {
            await Init();
            // Remove the selected land item from the database
            var deleteland = await db.DeleteAsync<Land>(id);
            return await Task.FromResult(true);
        }     
        public async Task<bool> UpdatelandAsync(Land item)
        {
            // modifying land item in the database
            await Init();
            var updateLand = await db.UpdateAsync(item);
            return await Task.FromResult(true);
        }

        public async Task<bool> AddUserlandAsync(User_Land item)
        {
            await Init();
            // create a new user land
            var userland = new User_Land
            {
                landId = item.landId,
                UserId = item.UserId,   
            };
            // add to the database
            await db.InsertAsync(userland);
            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateUserlandAsync(User_Land item)
        {
            // modifying userland item in the database
            await Init();
            var updateUserLand = await db.UpdateAsync(item);
            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteUserlandAsync(int id)
        {
            await Init();
            // Remove the selected userland item from the database
            var deleteuserland = await db.DeleteAsync<User_Land>(id);
            return await Task.FromResult(true);
        }

        public async Task<User_Land> GetUserlandAsync(int id)
        {
            await Init();
            // get the selected item 
            var userlandid = await db.Table<User_Land>().Where(d => d.Id == id).FirstOrDefaultAsync();
            return userlandid;
        }

        public async Task<IEnumerable<User_Land>> GetUserlandsAsync(bool forceRefresh = false)
        {
            await Init();
            // get all the users in the database
            var allUserlands = await db.Table<User_Land>().ToListAsync();
            return allUserlands;
        }

        public async Task<bool> AddNotificationAsync(Notification item)
        {
            await Init();
            // create a new user
            var notification = new Notification
            {
                Title = item.Title,
                Message = item.Message,
                Created = item.Created,
                userId = item.userId
            };
            // insert the values into the databases
            await db.InsertAsync(notification);
            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateNotificationAsync(Notification item)
        {
            // modifying notification item in the database
            await Init();
            var updateNotification = await db.UpdateAsync(item);
            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteNotificationAsync(int id)
        {
            await Init();
            // Remove the selected userland item from the database
            var deleteNotification = await db.DeleteAsync<Notification>(id);
            return await Task.FromResult(true);
        }
        public async Task<Notification> GetNotificationAsync(int id)
        {
            await Init();
            // get the selected item 
            var notification = await db.Table<Notification>().Where(d => d.Id == id).FirstOrDefaultAsync();
            return notification;
        }

        public async Task<IEnumerable<Notification>> GetNotificationsAsync(bool forceRefresh = false)
        {
            await Init();
            // get all the users in the database
            var Allnotifications = await db.Table<Notification>().ToListAsync();
            return Allnotifications;
        }
    }
}
    

