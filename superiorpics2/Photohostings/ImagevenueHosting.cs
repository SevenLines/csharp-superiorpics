using System;

namespace superiorpics
{
	public class ImagevenueHosting : Hosting
	{
		public override string HostingUrl {
			get {
				return @"http://(\w+)\.imagevenue.com/";
			}
		}

		public override string ImageXPath {
			get { 
				return "descendant-or-self::*[@id = 'thepic']";
			}
		}

		protected override string GetImageUrlFromNode (HtmlAgilityPack.HtmlNode root, Uri page_with_image_url)
		{
			var url = base.GetImageUrlFromNode (root, page_with_image_url);
			url = page_with_image_url.AbsoluteUri.Replace (page_with_image_url.PathAndQuery, "/"+url);
			return url;
		}
	}
}

