﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Plugin.Hud;
using Plugin.Hud.Abstractions;
using System.Globalization;
using CommunityCare.Resx;

namespace CommunityCare
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SurveyPage : ContentPage
	{
        string locale = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
		public SurveyPage ()
		{
			InitializeComponent ();
        }

        protected async override void OnAppearing() {
            
            surveyStack.Children.Clear();
            
            CrossHud.Current.Show(message: AppResource.Getting_Surveies, mask: MaskType.Clear);
            var httpClient = new HttpClient();
            var jsonString = await httpClient.GetStringAsync(String.Format(Contents.GetSurveyUrl, "Main%20Survey"));
            CrossHud.Current.Dismiss();

            Console.WriteLine(jsonString);
            var jsonArray = JArray.Parse(jsonString);

            foreach(var jsonObject in jsonArray){
                var questionStack = new StackLayout();
                var titleKey = (locale == "en") ? "etitle" : "atitle";
                var title = jsonObject[titleKey].ToString();
                questionStack.Children.Add(new Label { Text = title, FontSize = 20 });

                var qId = jsonObject["qID"].ToString();
                var qType = jsonObject["qType"].ToString();
                if(qType == "1")
                {
                    List<String> answerList = new List<String>();

                    for (int i = 1; i <= 12; i++)
                    {
                        var localPre = (locale == "en") ? "e" : "a";
                        var choice = jsonObject["choice" + i + localPre].ToString();
                        if (String.IsNullOrEmpty(choice)) break;
                        answerList.Add(choice);
                    }

                    RadioGroup radioGroup = new RadioGroup();
                    radioGroup.ClassId = qId;
                    radioGroup.IsRadio = true;
                    radioGroup.ItemsSource = answerList;
                    radioGroup.SelectedIndexes = new List<int>() { 0 };

                    questionStack.Children.Add(radioGroup);
                }
                else
                {
                    List<String> answerList = new List<String>();

                    for (int i = 1; i <= 12; i++)
                    {
                        var localPre = (locale == "en") ? "e" : "a";
                        var choice = jsonObject["choice" + i + localPre].ToString();
                        if (String.IsNullOrEmpty(choice)) break;
                        answerList.Add(choice);
                    }

                    RadioGroup radioGroup = new RadioGroup();
                    radioGroup.ClassId = qId;
                    radioGroup.IsRadio = false;
                    radioGroup.ItemsSource = answerList;
                    radioGroup.SelectedIndexes = new List<int>();

                    questionStack.Children.Add(radioGroup);
                }
                surveyStack.Children.Add(new Frame { Content = questionStack });
            }
        }

        async void OnFinishClicked(object sender, EventArgs args)
        {
            Console.WriteLine("Finish");
            String selectedIDstr = "";
            foreach(View child in surveyStack.Children)
            {
                if (child.GetType() != typeof(RadioGroup)) continue;
                var radioGroup = child as RadioGroup;
                var qId = radioGroup.ClassId;
                foreach (int selectedIndex in radioGroup.SelectedIndexes)
                {
                    selectedIDstr += qId + " " + selectedIndex + ",";
                }
            }
            selectedIDstr = selectedIDstr.Remove(selectedIDstr.Length - 1);

            var userId = Application.Current.Properties["clientID"] as String;

            String postUrl = String.Format(Contents.PostAnswersUrl, userId, 6, selectedIDstr);
            Console.WriteLine(postUrl);

            CrossHud.Current.Show(message: AppResource.Waiting, mask: MaskType.Clear);
            var httpClient = new HttpClient();
            var res = await httpClient.PostAsync(postUrl, null);
            CrossHud.Current.Dismiss();

            if (res.IsSuccessStatusCode)
            {
                var outputStr = await res.Content.ReadAsStringAsync();
                outputStr = outputStr.Replace("\"", "");
                await DisplayAlert(AppResource.CommCare, AppResource.Successfully_send_results_Recommend_Test + "\n" + outputStr, "OK");
                var paymentTypePage = new PaymentType();
                paymentTypePage.recommedation = outputStr;
                await Navigation.PushAsync(paymentTypePage);
            }
        }

        async void OnSkipClicked(object sender, EventArgs args)
        {

        }
    }
}