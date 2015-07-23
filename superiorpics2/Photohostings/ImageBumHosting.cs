using System;

namespace superiorpics
{
	public  class ImageBumHosting : Hosting
	{
		public override string HostingUrl {
			get {
				return "http://www.imagebam.com/";
			}
		}

		public override string ImageXPath {
			get { 
				return "descendant-or-self::*[@class and contains(concat(' ', normalize-space(@class), ' '), ' image ')]";
			}
		}

	}
}

