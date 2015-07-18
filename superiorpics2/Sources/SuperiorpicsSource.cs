using System;
using System.Linq;
using HtmlAgilityPack;

namespace superiorpics
{
	public class SuperiorpicsSource : Source
	{
		public override string ITEMS_XPATH {
			get { 
				return "descendant-or-self::*[@class and contains(concat(' ', normalize-space(@class), ' '), ' box135 ')]";
			}
		}
		public override string ITEM_LINK_XPATH {
			get { 
				return ".//a";
			}
		}
		public override string ITEM_TITLE_XPATH {
			get { 
				return "descendant-or-self::*[@class and contains(concat(' ', normalize-space(@class), ' '), ' forum-box-news-title-small ')]/descendant-or-self::*/a";
			}
		}
		public override string ITEM_THUMB_XPATH {
			get { 
				return "descendant-or-self::*[@class and contains(concat(' ', normalize-space(@class), ' '), ' cops-img ')]";
			}
		}

		public override string get_href(HtmlNode node) {
			var a_nodes = node.SelectNodes (ITEM_LINK_XPATH);
			var a = a_nodes.Where( (n) => {
				return n.Attributes["href"].Value.StartsWith("http://forums");
			}).First();
			if (a != null) {
				return a.Attributes ["href"].Value;
			}
			return "";
		}

		public SuperiorpicsSource ()
		{
		}
	}
}

