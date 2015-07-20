using System;
using Gdk;
using System.Net.Http;

namespace superiorpics
{
	[System.ComponentModel.ToolboxItem (true)]
	public partial class ImageLoader : Gtk.Bin
	{
		private Gtk.Image image = new Gtk.Image ();
		private string url = null;


		public PixbufAnimation loadAnimation = PixbufAnimation.LoadFromResource ("superiorpics.Resources.loading-icon.gif");


		public ImageLoader ()
		{
			this.Build ();
			this.Add (image);
			image.Show ();
			this.SetSizeRequest (100, 100);
		}

		private void StartLoadAnimation ()
		{
			this.image.PixbufAnimation = loadAnimation;
		}

		private void StopLoadAnimation (byte[] data = null)
		{
			this.image.PixbufAnimation = null;
			if (data != null) {
				this.image.Pixbuf = new Pixbuf(data);
			}
		}

		public string Url {
			get {
				return url;
			}
			set {
				StartLoadAnimation ();
				RequestHelper.getImage (value, (byte[] data) => {
					Gtk.Application.Invoke(delegate {
						StopLoadAnimation(data);
						url = value;
					});
				});
			}
		}
	}
}

