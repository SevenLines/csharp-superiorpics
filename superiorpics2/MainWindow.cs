using System;
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

	List<CelebrityItemJson> celebrities = null;
	WindowList wndCelebrityList = new WindowList ();

	public MainWindow () : base (Gtk.WindowType.Toplevel)
	{
		Build ();
		forumsGallery.PagesModel = pagesModel;
		forumsGallery.OnItemClicked = OpenLink;
		forumsGallery.OnPageChanged = SetPageUrl;

		var items_string = File.ReadAllText ("Resources/items.json");
		celebrities = JsonConvert.DeserializeObject<List<CelebrityItemJson>> (items_string)
			.OrderBy ((x) => x.Name).ToList ();

		wndCelebrityList.OnSelect += (string obj) => {
			edtQuery.Text = obj;
			Find();
		};

		this.FocusOutEvent += (o, args) => {
			wndCelebrityList.Hide ();
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

	protected void Find ()
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

	protected void OnBtnFindClicked (object sender, EventArgs e)
	{
		Find ();
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

	protected void ShowCelebritiesSelector ()
	{
		int dest_x, dest_y;
		edtQuery.GdkWindow.GetOrigin (out dest_x, out  dest_y);
		wndCelebrityList.Show ();	
		wndCelebrityList.Move (dest_x, dest_y + edtQuery.Allocation.Height);
		wndCelebrityList.Resize (edtQuery.Allocation.Width, 1);
	}

	protected void OnEdtQueryChanged (object sender, EventArgs e)
	{
		var name = edtQuery.Text;
		wndCelebrityList.Items = celebrities.Where ((x) => {
			return x.Name.ToLower ().Contains (name.ToLower ());
		}).Select ((x) => x.Name).Take (10).ToList ();
		ShowCelebritiesSelector ();
	}

	protected void OnEdtQueryKeyPressEvent (object o, KeyPressEventArgs args)
	{
		Console.WriteLine (args.Event.Key);
		switch (args.Event.Key) {
		case Gdk.Key.Down:
			args.RetVal = true;
			wndCelebrityList.SelectNext ();
			break;
		case Gdk.Key.Up:
			args.RetVal = true;
			wndCelebrityList.SelectPrevious ();
			break;
		case Gdk.Key.Escape:
			args.RetVal = true;
			wndCelebrityList.Hide ();
			break;
		}
	}

	protected void OnEdtQueryActivated (object sender, EventArgs e)
	{
		edtQuery.Text = wndCelebrityList.ActiveItem;
		wndCelebrityList.Hide ();
		Find ();
	}
}
