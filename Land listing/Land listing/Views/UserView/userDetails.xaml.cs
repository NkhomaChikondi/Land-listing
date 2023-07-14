using Land_listing.Models;
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
	public partial class userDetails : ContentPage
	{
		public userDetails (User user)
		{
			InitializeComponent();
			fullname.Text = user.FullName;
			username.Text = user.Username;
			blocked.Text = user.Blocked.ToString();
			phonenumber.Text = user.PhoneNumber;
			usertype.Text = user.Usertype;
			password.Text = user.Password;
		}
	}
}