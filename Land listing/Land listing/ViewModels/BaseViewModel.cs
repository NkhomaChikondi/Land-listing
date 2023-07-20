using Land_listing.Models;
using Land_listing.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace Land_listing.ViewModels
{
    public class BaseViewModel
    {
        public IDataUser<Models.User> dataUser { get; set; }
        public IdataLand<Models.Land> dataLand { get; set; }
        public IdataUserland<User_Land> dataUserLand { get; set; }
        public INotification<Notification> dataNotification { get; set; }
        public IToast Datatoast { get; }
        public BaseViewModel()
        {
            // exposing landlistingservice
            dataUser= DependencyService.Get<IDataUser<Models.User>>();
            dataLand = DependencyService.Get<IdataLand<Models.Land>>();
            Datatoast = DependencyService.Get<IToast>();
            dataUserLand = DependencyService.Get<IdataUserland<User_Land>>();
            dataNotification = DependencyService.Get<INotification<Notification>>();
        }
        bool isBusy;
        string Title;
        public string title
        {
            // get the title
            get => Title;
            // setting the Title with an incoming value
            set
            {
                if (Title == value)
                    return;
                Title = value;
                OnPropertyChange();
            }
        }
        public bool IsBusy
        {
            get => isBusy;
            set
            {
                if (isBusy == value)
                    return;
                isBusy = value;
                OnPropertyChange();
                OnPropertyChange(nameof(IsNotBusy));
            }
        }
        public bool IsNotBusy => !IsBusy;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChange([CallerMemberName] string Name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Name));
    }
}   

