using System;
using Gtk;
using HtmlAgilityPack;
using superiorpics;
using System.Text.RegularExpressions;
using System.Net;
using System.Threading;

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


	private bool loading = false;

	protected void OnBtnFindClicked (object sender, EventArgs e)
	{
		var url = String.Format (@"http://www.superiorpics.com/c/{0}/", Query ());
		if (loading)
			return;

		loading = true;


		Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
		RequestHelper.getRequestAsync (url, (data) => {
			Gtk.Application.Invoke(delegate	{
				Console.WriteLine(Thread.CurrentThread.ManagedThreadId);

				var doc = new HtmlDocument ();
				doc.LoadHtml (data);
				var root = doc.DocumentNode;

				var forums = source.get_forums (root);

				grid.RemoveAll ();
				foreach (var forum in forums) {
					var image = new ImageLoader ();
					//image.ParentWindow = this.ParentWindow;
					image.Url = forum.thumb;
					grid.AddWidget (image);
					image.Show ();
				}
				grid.Rebuild ();
				loading = false;
			});
		}, (ex) => {
			var response = (HttpWebResponse)ex.Response;
			Console.WriteLine (response.StatusCode);
			loading = false;
		});
	}
}
