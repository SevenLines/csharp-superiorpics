using System;
using System.Collections.Generic;
using Gtk;
using log4net;

namespace superiorpics
{
	[System.ComponentModel.ToolboxItem (true)]
	public partial class ImagePreviewWidget : Gtk.Bin
	{
		FileChooserDialog saveDialog;
		public static ILog log = LogManager.GetLogger (typeof(ImagePreviewWidget));

		public ImagePreviewWidget ()
		{
			this.Build ();
			saveDialog = new FileChooserDialog (
				"", 
				null, 
				FileChooserAction.Save,
				"Cancel", ResponseType.Cancel,
				"Save", ResponseType.Accept
			);

			imageloader.OnLoaded += (ImageLoader obj) => {
				imageloader.ShowButtons = true;
				UpdateTitle ();
			};

			imageloader.OnSaveClick += (string obj) => {
				saveDialog.CurrentName = System.IO.Path.GetFileName (imageloader.Url);
				if (saveDialog.Run () == (int)ResponseType.Accept) {
					imageloader.Image.Pixbuf.Save (saveDialog.Filename, "jpeg");
				}
				saveDialog.Hide ();
			};
		}

		protected override bool OnDeleteEvent (Gdk.Event evnt)
		{
			Hide ();
			return true;
		}

		protected void UpdateTitle ()
		{
			var fileName = System.IO.Path.GetFileName (imageloader.Url);
			imageloader.Label = String.Format (
				"{0}x{1} | <a href=\"{2}\">{3}</a>",
				imageloader.Image.Pixbuf.Width,
				imageloader.Image.Pixbuf.Height,
				imageloader.Url,
				fileName
			);
		}

		private void LoadImage ()
		{
			HostingManager.GetImageSrc (url, () => {
				imageloader.StartLoadAnimation ();
				imageloader.ShowButtons = false;
			}, (image_src) => {
				imageloader.Url = image_src;
			});
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

