using System;
using Gtk;

namespace superiorpics
{
	public partial class FullImagePreview : Gtk.Window
	{
		FileChooserDialog saveDialog;

		public FullImagePreview () :
			base (Gtk.WindowType.Toplevel)
		{
			this.Build ();
			saveDialog = new FileChooserDialog (
				"", 
				this, FileChooserAction.Save,
				"Cancel", ResponseType.Cancel,
				"Save", ResponseType.Accept
			);
		}

		protected override bool OnDeleteEvent (Gdk.Event evnt)
		{
			Hide ();
			return true;
		}

		private void LoadImage ()
		{
			imageloader.StartLoadAnimation ();
			var hosting = new ImageBumHosting ();
			if (hosting.IsHostingFor (url)) {
				imageloader.ShowButtons = false;
				hosting.GetImageUrl (url, (image_src) => {
					imageloader.Url = image_src;
					imageloader.ShowButtons = true;
				});
			}

			imageloader.OnSaveClick += (string obj) => {
				if (saveDialog.Run () == (int)ResponseType.Accept) {
					imageloader.Image.Pixbuf.Save(saveDialog.Filename, "jpeg");
				}
			};
		}

		private string url;

		public string Url {
			get {
				return url;
			}
			set {
				url = value;
				LoadImage ();
			}
		}
	}
}

