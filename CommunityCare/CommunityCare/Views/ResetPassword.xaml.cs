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

namespace CommunityCare
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ResetPassword : ContentPage
	{
        private String email;
		public ResetPassword (String email)
		{
			InitializeComponent ();
            this.email = email;
		}

        async void OnResetPasswordClicked(object sender, EventArgs args)
        {
            var newPassword = newPasswordEntry.Text;
            var confirmPassword = confirmPasswordEntry.Text;
            var pinNumber = pinNumberEntry.Text;

            if (!String.IsNullOrEmpty(newPassword) &&
                !String.IsNullOrEmpty(confirmPassword) &&
                !String.IsNullOrEmpty(pinNumber) &&
                newPassword == confirmPassword)
            {

                CrossHud.Current.Show(message: "Waiting...", mask: MaskType.Clear);
                var resetUrl = String.Format(Contents.ResetPasswordUrl, this.email, newPassword, pinNumber);
                Console.WriteLine(resetUrl);
                var httpClient = new HttpClient();
                var res = await httpClient.PostAsync(resetUrl, null);

                CrossHud.Current.Dismiss();

                if (res.IsSuccessStatusCode)
                {
                    var outputStr = await res.Content.ReadAsStringAsync();
                    outputStr = outputStr.Replace("\"", "");
                    var output = Convert.ToInt32(outputStr);
                    if (output > 0)
                    {
                        await DisplayAlert("ComCare!", "Reseted password successfully.", "OK");
                        await Navigation.PopToRootAsync();
                    }
                }

            }
            else
            {
                await DisplayAlert("Warning!", "Enter correctly.", "OK");
            }
        }
    }
}