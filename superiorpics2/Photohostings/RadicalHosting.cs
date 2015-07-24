using System;

namespace superiorpics
{
	public class RadicalHosting : Hosting
	{
		public override string ImageXPath {
			get {
				return "descendant-or-self::*[@class and contains(concat(' ', normalize-space(@class), ' '), ' f-content ')]/descendant-or-self::*/div[@itemtype]/descendant-or-self::*/img";
			}
		}

		public override string HostingUrl {
			get {
				return "http://radikal.ru/";
			}
		}
	}
}

