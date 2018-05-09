using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Plugin.Hud;
using Plugin.Hud.Abstractions;

namespace CommunityCare
{
	public partial class ContactPage : ContentPage
	{
        string website;
        string phoneNumber;
        string address;
        string email;

		public ContactPage()
		{
			InitializeComponent();
		}

        protected async override void OnAppearing()
        {
            CrossHud.Current.Show(message: "Getting info...", mask: MaskType.Clear);
            var httpClient = new HttpClient();
            var jsonString = await httpClient.GetStringAsync(Contents.GetContactInfo);
            CrossHud.Current.Dismiss();

            if (jsonString != "null")
            {
                var jsonObject = JObject.Parse(jsonString);
                website = jsonObject["WebSite"].ToString();
                email = jsonObject["Email"].ToString();
                address = jsonObject["Address"].ToString();
                phoneNumber = jsonObject["MobileNo"].ToString();

                websiteLabel.Text = website;
                emailLabel.Text = email;
                addressLabel.Text = address;
                phoneLabel.Text = phoneNumber;
            }
            else
            {
                await DisplayAlert("Warning!", "No info", "OK");
            }
        }

        async void OnClickedSend(object sender, EventArgs args)
        {
            var userId = Application.Current.Properties["clientID"] as String;
            var message = messageEditor.Text;
            if (!String.IsNullOrEmpty(message))
            {
                CrossHud.Current.Show(message: "Waiting...", mask: MaskType.Clear);
                var httpClient = new HttpClient();
                var res = await httpClient.PostAsync(String.Format(Contents.PostContactUS, userId, message), null);
                CrossHud.Current.Dismiss();
                if (res.IsSuccessStatusCode)
                {
                    await DisplayAlert("Success!", "We just sent contact message", "OK");
                    messageEditor.Text = "";
                }
            }
            else
            {
                await DisplayAlert("Warning!", "Enter correctly.", "OK");
            }

        }
    }
}
