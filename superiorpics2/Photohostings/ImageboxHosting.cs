using System;

namespace superiorpics
{
	public class ImageboxHosting : Hosting
	{
		public override string HostingUrl {
			get {
				return "http://imgbox.com/";
			}
		}

		public override string ImageXPath {
			get { 
				return "descendant-or-self::*[@id = 'img']";
			}
		}
	}
}

