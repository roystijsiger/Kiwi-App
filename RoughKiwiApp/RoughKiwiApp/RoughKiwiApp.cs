using System;
using System.Net.Http;
using Xamarin.Forms;

namespace RoughKiwiApp
{
	
	public class App : Application
	{
		public App ()
		{
			// The root page of your application
			MainPage = new ContentPage {
				Content = new StackLayout {
					VerticalOptions = LayoutOptions.Center,
					Children = {
						new Label {
							XAlign = TextAlignment.Center,
							Text = "Welcome to Xamarin Forms!"
						}
					}
				}
			};
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}

		private HttpClient _client = new HttpClient();
		private static string _url = "http://10.10.35.24/api/products";
		private ListView _listView;

	}

	public class Fruit{
		public int Id{get; set;}
		public string Name{get; set;}
		public string Color{get; set;}
	}
}

