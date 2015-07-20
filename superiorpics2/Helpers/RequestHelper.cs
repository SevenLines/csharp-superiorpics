using System;
using System.Net;
using System.Net.Http;
using System.IO;
using System.Threading.Tasks;
using log4net;

namespace superiorpics
{
	public class RequestHelper
	{
		public static ILog log = LogManager.GetLogger (typeof(RequestHelper));

		public RequestHelper ()
		{
		}

		public static string getRequest (string url)
		{
			var request = HttpWebRequest.Create (url);
			var response = (HttpWebResponse)request.GetResponse ();
			System.Console.Write (response.StatusCode);

			var stream = response.GetResponseStream ();
			var encoding = System.Text.Encoding.GetEncoding ("utf-8");

			var streamReader = new StreamReader (stream, encoding);

			string data = streamReader.ReadToEnd ();
			return data;
		}

		public static async void getRequestAsync (string url, Action<string> done, Action<WebException> fail)
		{
			log.Info (url);

			var request = HttpWebRequest.Create (url);
			HttpWebResponse response = null;
			try {
				response = (HttpWebResponse)await request.GetResponseAsync ();
			} catch (WebException ex) {
				if (fail != null) {
					fail (ex);
				}
			}

			if (response != null) {
				log.Info (response.StatusCode);

				var stream = response.GetResponseStream ();
				var encoding = System.Text.Encoding.GetEncoding ("utf-8");

				var streamReader = new StreamReader (stream, encoding);

				string data = streamReader.ReadToEnd ();
				if (done != null)
					done (data);
			}
		}

		public static async void getImage (string url, Action<byte[]> callback = null, Action<WebException> fail = null)
		{
			var client = new HttpClient ();
			byte[] data = null;
			try {
				data = await client.GetByteArrayAsync (url);
			} catch (WebException ex) {
				if (fail != null) {
					fail (ex);
				}
			}
			callback (data);
		}
	}
}

