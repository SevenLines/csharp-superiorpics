using System;
using Gdk;
using System.Net.Http;

namespace superiorpics
{
	[System.ComponentModel.ToolboxItem (true)]
	public partial class ImageLoader : Gtk.Bin
	{
//		private Gtk.Image image = new Gtk.Image ();
		private string url = null;


		public PixbufAnimation loadAnimation = PixbufAnimation.LoadFromResource ("superiorpics.Resources.loading-icon.gif");


		public ImageLoader ()
		{
			this.Build ();
//			this.Add (image);
//			image.Show ();
			this.SetSizeRequest (150, 150);
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


		public string Label {
			get {
				return this.label.LabelProp;
			}
			set {
				this.label.LabelProp = String.Format("<b>{0}</b>", value);
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

