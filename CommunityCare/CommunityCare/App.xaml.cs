using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using Newtonsoft.Json.Linq;
using CommunityCare.Resx;
using System.Globalization;

namespace CommunityCare
{
	public partial class App : Application
	{
		public App ()
		{
			InitializeComponent();

            //AppResource.Culture = new CultureInfo("ar");

            NavigationPage np;

            if (Application.Current.Properties.ContainsKey("is_logged_in"))
            {
                if (Application.Current.Properties["logged_user_type"].ToString() == "client")
                {
                    np = new NavigationPage(new MainPage());
                }
                else
                {
                    np = new NavigationPage(new StaffOrdersPage());
                }
            }
            else
            {
                np = new NavigationPage(new LoginPage());
            }
            np.BarBackgroundColor = Color.LightGray;
            np.BarTextColor = Color.Black;
            np.Title = "Community Health Care";
            MainPage = np;
            
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
