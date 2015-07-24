using System;
using HtmlAgilityPack;
using superiorpics;
using System.Text.RegularExpressions;
using System.Net;
using System.Threading;
using System.IO;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Reflection;
using log4net;

//Application settings wrapper class
using Gdk;


sealed class FormSettings : ApplicationSettingsBase
{
	[UserScopedSettingAttribute ()]
	public Gdk.Rectangle FormSize {
		get { return (Gdk.Rectangle)(this ["FormSize"]); }
		set { this ["FormSize"] = value; }
	}

	[UserScopedSettingAttribute ()]
	public Gdk.Rectangle WindowPreviewSize {
		get { return (Gdk.Rectangle)(this ["WindowPreviewSize"]); }
		set { this ["WindowPreviewSize"] = value; }
	}

	[UserScopedSettingAttribute ()]
	[DefaultSettingValueAttribute ("Images")]
	public string SaveDir {
		get { return (string)(this ["SaveDir"]); }
		set { this ["SaveDir"] = value; }
	}

	[UserScopedSettingAttribute ()]
	public string Query {
		get { return (string)(this ["Query"]); }
		set { this ["Query"] = value; }
	}
}

public partial class MainWindow: Gtk.Window
{
	private FormSettings settings = new FormSettings ();

	public static ILog log = LogManager.GetLogger (typeof(MainWindow));

	Source source = new SuperiorpicsSource ();
	Gtk.ListStore pagesModel = new Gtk.ListStore (typeof(string), typeof(string));

	List<CelebrityItemJson> celebrities = null;
	WindowList wndCelebrityList = new WindowList ();

	private bool Init = false;

	public MainWindow () : base (Gtk.WindowType.Toplevel)
	{
		Init = true;

		Build ();
		forumsGallery.PagesModel = pagesModel;
		forumsGallery.OnForumClick = OpenLink;
		forumsGallery.OnPageChanged = SetPageUrl;

		var items_string = File.ReadAllText ("Resources/items.json");
		celebrities = JsonConvert.DeserializeObject<List<CelebrityItemJson>> (items_string)
			.OrderBy ((x) => x.Name).ToList ();

		wndCelebrityList.OnSelect += (string obj) => {
			edtQuery.Text = obj;
			Find ();
		};

		galleryPreview.OnGalleryItemClick += (GalleryItem item) => {
			imagePreview.Url = item.url;
		};

		this.FocusOutEvent += (o, args) => {
			wndCelebrityList.Hide ();
		};

		LoadSettings ();

		Init = false;
	}

	protected void LoadSettings ()
	{
		settings.Reload ();
//		imagePreview.GdkWindow.Move (
//			settings.WindowPreviewSize.Left, 
//			settings.WindowPreviewSize.Top
//		);
//		imagePreview.GdkWindow.Resize (
//			settings.WindowPreviewSize.Width, 
//			settings.WindowPreviewSize.Height
//		);

		this.GdkWindow.Move (settings.FormSize.Left, settings.FormSize.Top);
		this.GdkWindow.Resize (settings.FormSize.Width, settings.FormSize.Height);
		this.edtQuery.Text = settings.Query;
		Find ();
	}

	protected void SaveSettings ()
	{
		int x, y;
		GdkWindow.GetOrigin (out x, out y);

		settings.FormSize = new Gdk.Rectangle (x, y, Allocation.Width, Allocation.Height);

//		imagePreview.GdkWindow.GetOrigin (out x, out y);
//		settings.WindowPreviewSize = new Gdk.Rectangle (
//			x, y, 
//			imagePreview.Allocation.Width, imagePreview.Allocation.Height
//		);
		settings.Query = edtQuery.Text;
		settings.Save ();
	}

	protected void OnDeleteEvent (object sender, Gtk.DeleteEventArgs a)
	{
		SaveSettings ();
		Gtk.Application.Quit ();
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
			log.Warn (response.StatusCode);
			loading = false;
		});
	}


	protected void OpenLink (ForumItem item)
	{
		log.Info (item);

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
		if (Init)
			return;
		var name = edtQuery.Text;
		wndCelebrityList.Items = celebrities.Where ((x) => {
			return x.Name.ToLower ().Contains (name.ToLower ());
		}).Select ((x) => x.Name).Take (10).ToList ();
		ShowCelebritiesSelector ();
	}

	protected void OnEdtQueryKeyPressEvent (object o, Gtk.KeyPressEventArgs args)
	{
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
		if (wndCelebrityList.ActiveItem != null) {
			edtQuery.Text = wndCelebrityList.ActiveItem;
		}
		wndCelebrityList.Hide ();
		Find ();
	}
}
