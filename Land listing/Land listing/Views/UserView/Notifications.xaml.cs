using Land_listing.Models;
using Land_listing.Services;
using Land_listing.ViewModels.Land;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Land_listing.Views.UserView
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Notifications : ContentPage
    {
        public INotification<Models.Notification> dataNotification { get; set; }
        User GetUser;
        public Notifications(User user)
        {
            InitializeComponent();
            BindingContext = new LandViewModel();
            dataNotification = DependencyService.Get<INotification<Models.Notification>>();
            GetUser = user;
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            //get all notifications
            await dataNotification.GetNotificationsAsync();
            var notifications = await dataNotification.GetNotificationsAsync();
            if(notifications.Count() == 0) 
            { 
               stackempty.IsVisible = true;
               stacklist.IsVisible = false;
            }
            else if(notifications.Count() > 0)
            {
                stackempty.IsVisible = false;
                stacklist.IsVisible = true;
            }
            if(BindingContext is LandViewModel viewModel)
            {
                await viewModel.LoadNotifications(GetUser);
            
            }
        }
    }
}