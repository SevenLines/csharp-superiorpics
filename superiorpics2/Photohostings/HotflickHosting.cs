using System;

namespace superiorpics
{
	public class HotflickHosting : Hosting
	{
		public override string HostingUrl {
			get {
				return "http://www.hotflick.net";
			}
		}

		public override string ImageXPath {
			get { 
				return "descendant-or-self::*[@id = 'img']";
			}
		}

		protected override string GetImageUrlFromNode (HtmlAgilityPack.HtmlNode root, Uri page_with_image_url)
		{
			var url = base.GetImageUrlFromNode (root, page_with_image_url);
			url = HostingUrl + url;
			return url;
		}
	}
}

