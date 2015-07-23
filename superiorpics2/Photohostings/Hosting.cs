﻿using System;
using HtmlAgilityPack;

namespace superiorpics
{
	public class Hosting
	{

		public virtual string HostingUrl {
			get {
				return "";
			}
		}

		public virtual string ImageXPath {
			get { 
				return "";
			}
		}

		/// <summary>
		/// Gets the image node.
		/// </summary>
		/// <returns>The image node.</returns>
		/// <param name="root">Root.</param>
		protected virtual HtmlNode GetImageNode (HtmlNode root)
		{
			return root.SelectSingleNode (ImageXPath);
		}

		/// <summary>
		/// Gets the image URL from node.
		/// </summary>
		/// <returns>The image URL from node.</returns>
		/// <param name="root">Root.</param>
		protected virtual string GetImageUrlFromNode (HtmlNode root)
		{
			var img_node = GetImageNode (root);
			return img_node.Attributes ["src"].Value;
		}

		/// <summary>
		/// Gets the image URL.
		/// </summary>
		/// <param name="page_with_image_url">Page with image URL.</param>
		/// <param name="callback">Callback.</param>
		public virtual void GetImageUrl (String page_with_image_url, Action<string> callback)
		{
			RequestHelper.getRequestAsync (page_with_image_url, (data) => {
				var doc = new HtmlDocument ();
				doc.LoadHtml (data);
				var url = GetImageUrlFromNode (doc.DocumentNode);
				if (callback != null) {
					callback (url);
				}
			}, null);
		}

		/// <summary>
		/// Should return true if this class can process this hosting
		/// </summary>
		public virtual bool IsHostingFor (string url)
		{
			return url.StartsWith(HostingUrl);
		}

	}
}

