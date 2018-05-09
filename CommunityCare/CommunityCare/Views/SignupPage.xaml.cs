using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net.Http;
using Plugin.Geolocator;
using Plugin.Hud;
using Plugin.Hud.Abstractions;

namespace CommunityCare
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SignupPage : ContentPage
	{
		public SignupPage ()
		{
			InitializeComponent ();
		}

        async void OnSignupClicked(object sender, EventArgs args)
        {
            bool isAccepted = this.checkAcceped.IsChecked;
            var email = emailAddressEntry.Text;
            var newPassword = newPasswordEntry.Text;
            var confirmPassword = confirmPasswordEntry.Text;

            if (isAccepted && 
                !String.IsNullOrEmpty(email) &&
                !String.IsNullOrEmpty(newPassword) &&
                !String.IsNullOrEmpty(confirmPassword) &&
                newPassword == confirmPassword)
            {

                CrossHud.Current.Show(message: "Waiting...", mask: MaskType.Clear);

                var locator = CrossGeolocator.Current;
                var position = await locator.GetPositionAsync(timeout: new TimeSpan(10000));

                var signupUrl = String.Format(Contents.SignupUrl, email, newPassword, "NoProvided", position.Latitude, position.Longitude, 1);
                Console.WriteLine(signupUrl);

                var httpClient = new HttpClient();
                var res = await httpClient.PostAsync(signupUrl, null);

                CrossHud.Current.Dismiss();

                if (res.IsSuccessStatusCode)
                {
                    await DisplayAlert("Welcome!", "Sign up successfully.", "OK");
                    await Navigation.PopAsync();
                }

            }
            else
            {
                await DisplayAlert("Warning!", "Enter correctly.", "OK");
            }
        }
    }
}