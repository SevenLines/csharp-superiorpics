using System;
using System.Net;
using System.Net.Http;
using System.IO;
using System.Threading.Tasks;

namespace superiorpics
{
	public class RequestHelper
	{
		public RequestHelper ()
		{
		}

		public static string getRequest(string url) 
		{
			var request = HttpWebRequest.Create (url);
			var response = (HttpWebResponse)request.GetResponse ();
			System.Console.Write (response.StatusCode);

			var stream = response.GetResponseStream ();
			var encoding = System.Text.Encoding.GetEncoding ("utf-8");

			var streamReader = new StreamReader(stream, encoding);

			string data = streamReader.ReadToEnd();
			return data;
		}

		public static async void getRequestAsync(string url, Action<string> callback) 
		{
			var request = HttpWebRequest.Create (url);
			var response = (HttpWebResponse) await request.GetResponseAsync ();
			System.Console.Write (response.StatusCode);

			var stream = response.GetResponseStream ();
			var encoding = System.Text.Encoding.GetEncoding ("utf-8");

			var streamReader = new StreamReader(stream, encoding);

			string data = streamReader.ReadToEnd();
			callback (data);
		}

		public static async void getImage(string url, Action<byte[]> callback)
		{
			var client = new HttpClient ();
			byte[] data = await client.GetByteArrayAsync (url);
			callback (data);
		}
	}
}

