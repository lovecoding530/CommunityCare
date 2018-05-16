using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.GoogleMaps;
using Plugin.Geolocator;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Plugin.Hud;
using Plugin.Hud.Abstractions;
using CommunityCare.Resx;

namespace CommunityCare
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TimeAndLocation : ContentPage
	{
        public string surveyRecommedation;
        public string orderItems;
        public int paymentTypeIndex;

        Pin pin = new Pin()
        {
            Type = PinType.Place,
            Label = "Selected Location",
            IsVisible = true
        };

        public TimeAndLocation()
		{
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            var locator = CrossGeolocator.Current;
            
            if(locator.IsGeolocationAvailable || locator.IsGeolocationEnabled)
            {
                var position = await locator.GetPositionAsync(TimeSpan.FromTicks(10000));
                pin.Position = new Position(position.Latitude, position.Longitude);

                map.Pins.Add(pin);
                map.MoveToRegion(MapSpan.FromCenterAndRadius(pin.Position, Distance.FromMeters(5000)));
            }

            map.MapClicked += (sender, e) =>
            {
                var lat = e.Point.Latitude;
                var lng = e.Point.Longitude;
                pin.Position = new Position(lat, lng);
                map.MoveToRegion(MapSpan.FromCenterAndRadius(pin.Position, Distance.FromMeters(5000)));
            };
        }

        async void OnClickedConfirm(object sender, EventArgs args)
        {
            var userId = Application.Current.Properties["clientID"] as String;
            var dateStr = datePicker.Date.ToString("yyyy-MM-dd");
            var timeStr = timePicker.Time.ToString("g");
            var location = pin.Position;
            var recommendation = "no%20recommendation";

            String postUrl;
            if (string.IsNullOrEmpty(surveyRecommedation))
            {
                postUrl = String.Format(Contents.CreateOrderByItems,
                                        userId,
                                        dateStr, timeStr,
                                        location.Longitude, location.Latitude,
                                        paymentTypeIndex,
                                        recommendation,
                                        orderItems);
            }
            else
            {
                postUrl = String.Format(Contents.CreateOrderBySurveyRecomendation,
                                        userId,
                                        dateStr, timeStr,
                                        location.Longitude, location.Latitude,
                                        paymentTypeIndex,
                                        recommendation,
                                        surveyRecommedation);
            }

            CrossHud.Current.Show(message: AppResource.Waiting, mask: MaskType.Clear);
            var httpClient = new HttpClient();
            var res = await httpClient.PostAsync(postUrl, null);
            CrossHud.Current.Dismiss();

            if (res.IsSuccessStatusCode)
            {
                var outputStr = await res.Content.ReadAsStringAsync();
                if(outputStr == "true")
                {
                    await DisplayAlert(AppResource.Success, AppResource.You_will_recieve_the_order_confirmation_on_your_email, "OK");
                    await Navigation.PopToRootAsync();
                }
            }
        }

        async void OnClickedCancel(object sender, EventArgs args)
        {
            var answer = await DisplayAlert(AppResource.CommCare, AppResource.Are_you_sure_to_cancel_the_Lab_Test, "Yes", "No");
            if (answer)
            {
                await Navigation.PopAsync();
            }
        }
    }
}