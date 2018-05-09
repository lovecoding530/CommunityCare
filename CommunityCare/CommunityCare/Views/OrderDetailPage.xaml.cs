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
using CommunityCare.Models;

namespace CommunityCare
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class OrderDetailPage : ContentPage
	{
        public Order order;

        Pin pin = new Pin()
        {
            Type = PinType.Place,
            Label = "Selected Location",
            IsVisible = true
        };

        public OrderDetailPage()
		{
            InitializeComponent();
            map.Pins.Add(pin);
            testsListView.ItemTapped += (object sender, ItemTappedEventArgs e) => {
                // don't do anything if we just de-selected the row
                if (e.Item == null) return;
                // do something with e.SelectedItem
                ((ListView)sender).SelectedItem = null; // de-select the row
            };
        }

        protected async override void OnAppearing()
        {
            var locator = CrossGeolocator.Current;

            paymentTypeLabel.Text = Contents.paymentTypes[order.paymentType];
            dateTimeLabel.Text = order.orderDateTimeStr;
            pin.Position = new Position(order.latitude, order.longitude);
            map.MoveToRegion(MapSpan.FromCenterAndRadius(pin.Position, Distance.FromMeters(5000)));
            testsListView.ItemsSource = order.labTests;

        }
    }
}