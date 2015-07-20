using System;
using System.Linq;
using System.Collections.Generic;
using HtmlAgilityPack;

namespace superiorpics
{
	public class PageItem
	{
		public int page;
		public string url;
		public string title;
	};

	public class ForumItem
	{
		public string title;
		public string url;
		public string thumb;
		public List<PageItem> pages;

		public override string ToString ()
		{
			return string.Format ("[ForumItem]{0}| url{1}", title, url);
		}
	};

	public class Source
	{
		public virtual string ROOT_URL {
			get { 
				return "";
			}
		}

		public virtual string ITEMS_XPATH {
			get { 
				return "";
			}
		}

		public virtual string ITEM_LINK_XPATH {
			get { 
				return "";
			}
		}

		public virtual  string ITEM_TITLE_XPATH {
			get { 
				return "";
			}
		}

		public virtual string ITEM_THUMB_XPATH {
			get { 
				return "";
			}
		}

		public virtual string ITEM_PAGER_XPATH {
			get { 
				return "";
			}
		}

		public Source ()
		{
		}

		public virtual List<ForumItem> get_forums (HtmlNode root, List<PageItem> pages = null)
		{
			var nodes = get_items_nodes (root);

			if (pages!=null) {
				var pager_node = get_pager (root);
				get_pages (pager_node, pages);
			}

			if (nodes != null) {
				return nodes.Select ((node) => {
					return new ForumItem {
						url = get_href (node),
						title = get_title (node),
						thumb = get_thumb (node),
					};
				}).ToList ();
			} else {
				return new List<ForumItem> ();
			}
		}


		public virtual HtmlNodeCollection get_items_nodes (HtmlNode root)
		{
			return root.SelectNodes (this.ITEMS_XPATH);
		}

		public virtual string get_href (HtmlNode node)
		{
			var _node = node.SelectSingleNode (ITEM_LINK_XPATH);
			return _node.GetAttributeValue ("href", "");
		}

		public virtual string get_title (HtmlNode node)
		{
			var _node = node.SelectSingleNode (ITEM_TITLE_XPATH);
			return _node.InnerText;
		}

		public virtual string get_thumb (HtmlNode node)
		{
			var _node = node.SelectSingleNode (ITEM_THUMB_XPATH);
			return _node.GetAttributeValue ("src", "");
		}

		public virtual HtmlNode get_pager (HtmlNode root)
		{
			return root.SelectSingleNode (ITEM_PAGER_XPATH); 
		}

		public virtual void get_pages (HtmlNode pager_node, List<PageItem> pages = null)
		{
			throw new NotImplementedException ();
		}
	}
}

