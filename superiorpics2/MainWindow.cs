using System;
using Gtk;
using HtmlAgilityPack;
using superiorpics;
using System.Text.RegularExpressions;
using System.Net;

public partial class MainWindow: Gtk.Window
{
	Source source = new SuperiorpicsSource ();

	public MainWindow () : base (Gtk.WindowType.Toplevel)
	{
		Build ();
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

	protected void OnBtnFindClicked (object sender, EventArgs e)
	{
		var url = String.Format (@"http://www.superiorpics.com/c/{0}/", Query ());

		RequestHelper.getRequestAsync (url, (data) => {
			var doc = new HtmlDocument ();
			doc.LoadHtml (data);
			var root = doc.DocumentNode;

			var forums = source.get_forums (root);
			foreach (var forum in forums) {
				Console.WriteLine (forum.thumb);
			}
		}, (ex) => {
			var response = (HttpWebResponse)ex.Response;
			Console.WriteLine (response.StatusCode);
		});
		Console.WriteLine ("out");
	}
}
