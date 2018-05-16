using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using ImageButton;
using CommunityCare.Models;
using Plugin.Hud;
using Plugin.Hud.Abstractions;
using CommunityCare.Resx;

namespace CommunityCare
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderHistoryPage : ContentPage
    {

        ObservableCollection<Order> orders = new ObservableCollection<Order>();

        public OrderHistoryPage()
        {
            InitializeComponent();

            listView.ItemsSource = orders;
            listView.ItemTapped += async (object sender, ItemTappedEventArgs e) => {
                // don't do anything if we just de-selected the row
                if (e.Item == null) return;
                // do something with e.SelectedItem
                var item = listView.SelectedItem as Order;

                var detailPage = new OrderDetailPage();
                detailPage.order = item;
                await Navigation.PushAsync(detailPage);

                listView.SelectedItem = null; // de-select the row
            };

        }

        protected async override void OnAppearing()
        {
            orders.Clear();
            var userId = Application.Current.Properties["clientID"] as String;

            CrossHud.Current.Show(message: AppResource.Getting_tests, mask: MaskType.Clear);
            var httpClient = new HttpClient();
            var jsonString = await httpClient.GetStringAsync(String.Format(Contents.GetAllUserOrders, userId));
            CrossHud.Current.Dismiss();

            Console.WriteLine(jsonString);
            var jsonArray = JArray.Parse(jsonString);

            foreach (var jsonObject in jsonArray)
            {
                orders.Add(new Order(jsonObject));
            }
        }
    }
}