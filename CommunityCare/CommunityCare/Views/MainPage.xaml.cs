using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CommunityCare
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
            
        }

        async void OnClickedQuickSurvey(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new SurveyPage());
        }

        async void OnClickedSkipSurvey(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new ManualLabTest());
        }

        async void OnClickedAllSurveys(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new SurveyHistoryPage());
        }

        async void OnClickedAllLabtests(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new OrderHistoryPage());
        }

        async void OnClickedContactUs(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new ContactPage());
        }
    }
}
