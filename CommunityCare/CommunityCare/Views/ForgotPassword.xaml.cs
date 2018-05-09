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
	public partial class ForgotPassword : ContentPage
	{
		public ForgotPassword ()
		{
			InitializeComponent ();
		}

        async void OnRetrievePasswordClicked(object sender, EventArgs args)
        {

            var email = emailAddressEntry.Text;
            if(!String.IsNullOrEmpty(email))
            {
                CrossHud.Current.Show(message: "Waiting...", mask: MaskType.Clear);
                var httpClient = new HttpClient();
                var res = await httpClient.PostAsync(String.Format(Contents.ForgotPasswordUrl, email), null);
                CrossHud.Current.Dismiss();
                if (res.IsSuccessStatusCode)
                {
                    await DisplayAlert("Success!", "We just sent an email with Pin code", "OK");
                    await Navigation.PushAsync(new ResetPassword(email: email));
                }
            }
            else
            {
                await DisplayAlert("Warning!", "Enter correctly.", "OK");
            }
        }
    }
}