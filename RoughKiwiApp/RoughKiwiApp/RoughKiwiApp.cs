using System;
using System.Net.Http;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace RoughKiwiApp
{

	public class App : Application
	{
		public App ()
		{
			// The root page of your application
			MainPage = new MainXaml();
			_listView = MainPage.FindByName<ListView> ("listviewProducts");
		}

		private async Task<IEnumerable<Product>> GetListviewFromTheApi(){
			var contentTask = _client.GetAsync(_url);
			var content = await contentTask;
			var jsonReturn = await content.Content.ReadAsStringAsync ();
			return JsonConvert.DeserializeObject<IEnumerable<Product>>(jsonReturn);
		}
		protected override async void OnStart ()
		{
			// Handle when your app starts
			_listView.ItemsSource = await GetListviewFromTheApi();
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
		private static string _url = "http://10.10.35.24/reports/";
		private ListView _listView;

	}		
}
	