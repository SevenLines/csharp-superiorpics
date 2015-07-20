using System;
using System.Linq;
using HtmlAgilityPack;
using System.Collections.Generic;

namespace superiorpics
{
	public class SuperiorpicsSource : Source
	{
		public override string ROOT_URL {
			get { 
				return "http://www.superiorpics.com";
			}
		}

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

		public override string ITEM_PAGER_XPATH {
			get { 
				return "descendant-or-self::*[@class and contains(concat(' ', normalize-space(@class), ' '), ' alphaGender-font-paging-box4 ')]";
			}
		}

		public override string get_href (HtmlNode node)
		{
			var a_nodes = node.SelectNodes (ITEM_LINK_XPATH);
			var a = a_nodes.Where ((n) => {
				return n.Attributes ["href"].Value.StartsWith ("http://forums");
			}).First ();
			if (a != null) {
				return a.Attributes ["href"].Value;
			}
			return "";
		}

		public override void get_pages (HtmlNode pager_node, List<PageItem> pages)
		{
			if (pages == null) {
				return;
			}

			int max = 0;
			var base_url = "";
			foreach (var node in pager_node.SelectNodes(".//a")) {
				try {
					int value = Convert.ToInt32 (node.InnerText);
					if (value == 1) {
						base_url = String.Format("{0}{1}", ROOT_URL, node.Attributes["href"].Value);
						base_url = base_url.Replace(".html", "");
					}
					if (value > max) {
						max = value;
					}	
				} catch (FormatException) {
				}
			}

			foreach (var i in Enumerable.Range(1, max)) {
				var url = String.Format ("{0}{1}.html", base_url, i == 1 ? "" : i.ToString ());
				var item = new PageItem {
					page = i,
					url = url
				};
				pages.Add (item);
			}

			return;
		}

		public SuperiorpicsSource ()
		{
		}
	}
}

