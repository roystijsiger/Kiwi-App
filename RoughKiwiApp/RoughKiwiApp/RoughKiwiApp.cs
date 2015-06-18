using System;
using System.Net.Http;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Diagnostics;
using System.Collections;

namespace RoughKiwiApp
{

	public class App : Application
	{
		public App ()
		{
			// The root page of your application
			MainPage = new MainXaml();

			_label = MainPage.FindByName<Label>("label1");
			_listview = MainPage.FindByName<ListView> ("listview1");
			//listview _listView = MainPage.FindByName<ListView> ("listviewProducts");
		}

		/*listview private async Task<IEnumerable<Report>> GetListviewFromTheApi(){
			var contentTask = _client.GetAsync(_url);
			var content = await contentTask;
			var jsonReturn = await content.Content.ReadAsStringAsync ();
			return JsonConvert.DeserializeObject<IEnumerable<Report>>(jsonReturn);
		}*/

		protected override async void OnStart ()
		{
			// Handle when your app starts
			//listview _listView.ItemsSource = await GetListviewFromTheApi();
	
			await Authorize("Hbd", "masterpass");
			var content = await _client.GetAsync(_urlReportsGet);
			var json = await content.Content.ReadAsStringAsync();
			var reportsList = JsonConvert.DeserializeObject<IEnumerable<Report>>(json);

			_listview.ItemsSource = reportsList;
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}

		private async Task Authorize(string username, string password){
			var authenticator = new Authenticator();
			var authToken = await authenticator.Login(username, password);
			var authTokenString = string.Format("{0} {1}", authToken.Type, authToken.Value);

			_client.DefaultRequestHeaders.Add ("Authorization",authTokenString);
		}
		//private ListView _listView;
		private Label _label;
		private ListView _listview;
		private HttpClient _client = new HttpClient();
		private static string _urlReportsGet = "http://leerpark-api-develop.azurewebsites.net/reports/";

	}

	public class Authenticator
	{
		public async Task<Token> Login(string username, string password)
		{
			var requestContent = string.Format("grant_type=password&username={0}&password={1}",
				//todo urlencodes
				username, 
				password);
			
			return await Login(requestContent);
		}

		public async Task<Token> Login(string requestContent)
		{
			var request = new HttpRequestMessage
			{
				Method = HttpMethod.Post,
				RequestUri = new Uri(_urlAuth),
				Content = new StringContent(requestContent)
			};

			HttpResponseMessage result;

			try
			{
				result = await _client.SendAsync(request);
			}
			catch (Exception e)
			{
				Debug.WriteLine (e.Message);
				return null;
			}

			if (result.StatusCode != HttpStatusCode.OK) {
				return null;
			}

			var resultContent = await result.Content.ReadAsStringAsync();
			var res = JObject.Parse(resultContent);
			var token = new Token
			{
				ExpiresIn = res.SelectToken("expires_in").Value<int>(),
				Role = res.SelectToken("role").Value<string>(),
				Value = res.SelectToken("access_token").Value<string>(),
				Type = res.SelectToken("token_type").Value<string>()
			};
					
			Debug.WriteLine (token.Value);
			return token;		
		}

		private HttpClient _client = new HttpClient();
		private static string _urlAuth = "http://leerpark-api-develop.azurewebsites.net/api/oauth";
	}
}
	