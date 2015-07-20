using System;
using Gtk;
using HtmlAgilityPack;
using superiorpics;
using System.Text.RegularExpressions;
using System.Net;
using System.Threading;
using System.Collections.Generic;

public partial class MainWindow: Gtk.Window
{
	Source source = new SuperiorpicsSource ();
	ListStore pagesModel = new ListStore (typeof(string), typeof(string));

	public MainWindow () : base (Gtk.WindowType.Toplevel)
	{
		Build ();
		forumsGallery.PagesModel = pagesModel;
		forumsGallery.OnItemClicked = OpenLink;
		forumsGallery.OnPageChanged = SetPageUrl;
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

			var gallery_items = source.get_gallery(root);
			galleryPreview.SetGallery(gallery_items);

			notebook.CurrentPage = 1;
//			var gallery = new Gallery ();
//			gallery.SetGallery(gallery_items);
//			gallery.Show ();

//			var label = new Label (item.title);
//			label.Show ();
//			notebook.AppendPage (gallery, label);
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
}
