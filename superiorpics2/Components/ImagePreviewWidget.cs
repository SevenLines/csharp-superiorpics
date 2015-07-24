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

		Hosting[] Hostings = new Hosting[]{
			new ImageBumHosting (),
			new ImagevenueHosting ()
		};

		public ImagePreviewWidget ()
		{
			this.Build ();
			//			saveDialog = new FileChooserDialog (
			//				"", 
			//				this.GdkWindow, 
			//				FileChooserAction.Save,
			//				"Cancel", ResponseType.Cancel,
			//				"Save", ResponseType.Accept
			//			);

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
			//			this.Title = String.Format (
			//				"{0}x{1} | {2}",
			//				imageloader.Image.Pixbuf.Width,
			//				imageloader.Image.Pixbuf.Height,
			//				imageloader.Url
			//			);
		}

		private void LoadImage ()
		{
			log.Info ("Try to load: " + url);
			foreach (var hosting in Hostings) {
				if (hosting.IsHostingFor (url)) {
					log.Info ("Hosting is: " + hosting.GetType().Name);
					log.Debug ("Start loading");
					imageloader.StartLoadAnimation ();
					imageloader.ShowButtons = false;
					hosting.GetImageUrl (url, (image_src) => {
						log.Debug ("Successfully get image src: " + image_src);
						imageloader.Url = image_src;
					});
					return;
				}
			}
			log.Warn ("Failed to find hosting loader for: " + url);
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

