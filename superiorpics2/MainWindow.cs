using System;
using Gtk;
using HtmlAgilityPack;
using superiorpics;

public partial class MainWindow: Gtk.Window
{
	Source source = new SuperiorpicsSource();

	public MainWindow () : base (Gtk.WindowType.Toplevel)
	{
		Build ();
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	protected void OnBtnFindClicked (object sender, EventArgs e)
	{
		RequestHelper.getRequestAsync (
			@"http://www.superiorpics.com/c/Alison_Brie/",
			(data) => {
				var doc = new HtmlDocument();
				doc.LoadHtml(data);
				var root = doc.DocumentNode;

				var forums = source.get_forums(root);
				foreach(var forum in forums) {
					Console.WriteLine(forum.thumb);
				}
			}
		);
		Console.WriteLine ("out");
	}
}
