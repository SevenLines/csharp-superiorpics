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

		var renderer = new CellRendererText ();
		cmbPage.PackStart (renderer, false);
		cmbPage.AddAttribute (renderer, "text", 1);

		cmbPage.WrapWidth = 10;
		cmbPage.Model = pagesModel;
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

	public void getUrl (string url, Action<string> done=null)
	{
		RequestHelper.getRequestAsync (url, (data) => {
			Gtk.Application.Invoke (delegate {
				if (done!=null) {
					done (data);
				}
			});
		}, (ex) => {
			var response = (HttpWebResponse)ex.Response;
			Console.WriteLine (response.StatusCode);
			loading = false;
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

			grid.RemoveAll ();
			foreach (var forum in forums) {
				var image = new ImageLoader ();
				image.Url = forum.thumb;
				image.Label = forum.title;
				grid.AddWidget (image);
				image.Show ();
			}
			grid.Rebuild ();
			loading = false;
		});
	}

	protected void OnCmbPageChanged (object sender, EventArgs e)
	{
		if (cmbPage.ActiveText != null) {
			getUrl (cmbPage.ActiveText, (data) => {
				var doc = new HtmlDocument ();
				doc.LoadHtml (data);
				var root = doc.DocumentNode;

				var forums = source.get_forums (root);

				grid.RemoveAll ();
				foreach (var forum in forums) {
					var image = new ImageLoader ();
					image.Url = forum.thumb;
					image.Label = forum.title;
					grid.AddWidget (image);
					image.Show ();
				}
				grid.Rebuild ();
				loading = false;
			});
		}
	}
}
