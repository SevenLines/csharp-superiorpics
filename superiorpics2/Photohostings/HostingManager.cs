using System;
using log4net;

namespace superiorpics
{
	public class HostingManager
	{
		private static ILog log = LogManager.GetLogger (typeof(HostingManager));

		private static Hosting[] Hostings = new Hosting[] {
			new ImageBumHosting (),
			new ImagevenueHosting (),
			new HotflickHosting (),
			new ImageboxHosting (),
			new RadicalHosting ()
		};

		public static void GetImageSrc (
			String url, 
			Action OnBeforeLoad = null, 
			Action<string> OnSrcGet = null
		)
		{
			log.Info ("Try to load: " + url);
			foreach (var hosting in Hostings) {
				if (hosting.IsHostingFor (url)) {
					log.Info ("Hosting is: " + hosting.GetType ().Name);
					log.Debug ("Start loading");
					if (OnBeforeLoad != null) {
						OnBeforeLoad ();
					}
					hosting.GetImageUrl (url, (image_src) => {
						log.Debug ("Successfully get image src: " + image_src);
						if (OnSrcGet != null) {
							OnSrcGet (image_src);
						}
					});
					return;
				}
			}
			log.Warn ("Failed to find hosting loader for: " + url);
		}

		public static void GetImage (
			String url, 
			Action OnBeforeLoad = null, 
			Action<byte[], string> OnImageGet = null
		)
		{
			GetImageSrc (url, OnBeforeLoad, (image_src) => {
				RequestHelper.getImage(image_src, (data) => {
					if (OnImageGet!=null) {
						OnImageGet(data, image_src);
					}
				});
			});
		}
	}
}

