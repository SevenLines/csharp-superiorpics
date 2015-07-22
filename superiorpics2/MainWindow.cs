﻿using System;
using Gtk;
using HtmlAgilityPack;
using superiorpics;
using System.Text.RegularExpressions;
using System.Net;
using System.Threading;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Reflection;

public partial class MainWindow: Gtk.Window
{
	Source source = new SuperiorpicsSource ();
	ListStore pagesModel = new ListStore (typeof(string), typeof(string));

	//	TreeModelFilter celebrityModelProxy = null;
	List<CelebrityItemJson> celebrities = null;
	//	int limitCount = 0;
	WindowList window = new WindowList();

	public MainWindow () : base (Gtk.WindowType.Toplevel)
	{
		Build ();
		forumsGallery.PagesModel = pagesModel;
		forumsGallery.OnItemClicked = OpenLink;
		forumsGallery.OnPageChanged = SetPageUrl;

		var items_string = File.ReadAllText ("Resources/items.json");
		celebrities = JsonConvert.DeserializeObject<List<CelebrityItemJson>> (items_string)
			.OrderBy((x) => x.Name).ToList();

		window.OnSelect += (string obj) => {
			edtQuery.Text = obj;
			btnFind.Click();
		};
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	protected string Query ()
	{
		var query = edtQuery.Text;
		query = query.Trim ();
		query = Regex.Replace (query, @"\s+", "_");
		return query;
	}


	private bool loading = false;

	public void getUrl (string url, Action<string> done = null)
	{
		RequestHelper.getRequestAsync (url, (data) => {
			Gtk.Application.Invoke (delegate {
				if (done != null) {
					done (data);
				}
			});
		}, (ex) => {
			var response = (HttpWebResponse)ex.Response;
			Console.WriteLine (response.StatusCode);
			loading = false;
		});
	}


	protected void OpenLink (ForumItem item)
	{
		Console.WriteLine (item);

		getUrl (item.url, (data) => {
			var doc = new HtmlDocument ();
			doc.LoadHtml (data);
			var root = doc.DocumentNode;

			var gallery_items = source.get_gallery (root);
			galleryPreview.SetGallery (gallery_items);

			notebook.CurrentPage = 1;
		});
	}

	protected void OnBtnFindClicked (object sender, EventArgs e)
	{
		var url = String.Format (@"http://www.superiorpics.com/c/{0}/", Query ());
		if (loading)
			return;

		loading = true;

		getUrl (url, (data) => {
			var doc = new HtmlDocument ();
			doc.LoadHtml (data);
			var root = doc.DocumentNode;

			var pages = new List<PageItem> ();
			var forums = source.get_forums (root, pages);

			pagesModel.Clear ();
			foreach (var page in pages) {
				pagesModel.AppendValues (page.url, page.page.ToString ());
			}
			forumsGallery.SetForums (forums);

			loading = false;
		});
	}

	protected void SetPageUrl (int page, string pageText)
	{
		if (pageText != null) {
			getUrl (pageText, (data) => {
				var doc = new HtmlDocument ();
				doc.LoadHtml (data);
				var root = doc.DocumentNode;

				var forums = source.get_forums (root);

				forumsGallery.SetForums (forums);

				loading = false;
			});
		}
	}


	protected void OnEdtQueryChanged (object sender, EventArgs e)
	{
		var name = edtQuery.Text;
		window.Items = celebrities.Where ((x) => x.Name.Contains (name))
			.Select((x) => x.Name).Take (10).ToList();
	}

	protected void OnEdtQueryKeyReleaseEvent (object o, KeyReleaseEventArgs args)
	{
		window.Show ();

		int dest_x, dest_y;
		edtQuery.GdkWindow.GetOrigin (out dest_x,out  dest_y);
		window.Move (dest_x, dest_y + edtQuery.Allocation.Height);
		window.Resize (edtQuery.Allocation.Width, 1);
	}

	protected void OnEdtQueryKeyPressEvent (object o, KeyPressEventArgs args)
	{
		Console.WriteLine (args.Event.Key);
	}
}
