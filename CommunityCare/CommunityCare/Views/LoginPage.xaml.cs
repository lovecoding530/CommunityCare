using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net.Http;
using Plugin.Hud;
using Plugin.Hud.Abstractions;
using Newtonsoft.Json.Linq;
using CommunityCare.Resx;
using System.Globalization;

namespace CommunityCare
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{
		public LoginPage ()
		{
			InitializeComponent ();
        }

        async void OnSigninClicked(object sender, EventArgs args)
        {
            var email = emailAddressEntry.Text;
            var password = passwordEntry.Text;
            if(!String.IsNullOrEmpty(email) && !String.IsNullOrEmpty(password))
            {
                CrossHud.Current.Show(message: "Signing in", mask: MaskType.Clear);
                var httpClient = new HttpClient();
                var jsonString = await httpClient.GetStringAsync(String.Format(Contents.LoginUrl, email, password));
                CrossHud.Current.Dismiss();

                if (jsonString != "null")
                {
                    var jsonObject = JObject.Parse(jsonString);
                    if(jsonObject["clientID"] != null)
                    {

                        Application.Current.Properties["is_logged_in"] = true;
                        Application.Current.Properties["logged_user_type"] = "client";
                        Application.Current.Properties["username"] = jsonObject["username"].ToString();
                        Application.Current.Properties["password"] = jsonObject["password"].ToString();
                        Application.Current.Properties["clientID"] = jsonObject["clientID"].ToString();
                        //await Navigation.PushAsync(new MainPage());

                        var np = new NavigationPage(new MainPage());
                        np.BarBackgroundColor = Color.LightGray;
                        np.BarTextColor = Color.Black;
                        np.Title = "Community Health Care";
                        App.Current.MainPage = np;

                    }
                    else
                    {
                        Application.Current.Properties["is_logged_in"] = true;
                        Application.Current.Properties["logged_user_type"] = "staff";
                        Application.Current.Properties["username"] = jsonObject["username"].ToString();
                        Application.Current.Properties["password"] = jsonObject["password"].ToString();
                        Application.Current.Properties["staffID"] = jsonObject["staffID"].ToString();
                        //await Navigation.PushAsync(new MainPage());

                        var np = new NavigationPage(new StaffOrdersPage());
                        np.BarBackgroundColor = Color.LightGray;
                        np.BarTextColor = Color.Black;
                        np.Title = "Community Health Care";
                        App.Current.MainPage = np;

                    }

                }
                else
                {
                    await DisplayAlert("Warning!", "Invalid email or password", "OK");
                }
            }
            else
            {
                await DisplayAlert("Warning!", "Enter correctly.", "OK");
            }
        }

        async void OnForgotPasswordClicked(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new ForgotPassword());
        }

        async void OnSignupClicked(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new SignupPage());
        }
    }
}