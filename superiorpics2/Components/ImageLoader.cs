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
		private string link = "";

		public PixbufAnimation loadAnimation = PixbufAnimation.LoadFromResource ("superiorpics.Resources.loading-icon.gif");
		public Action<ImageLoader> OnLoadFailed;

		public ImageLoader ()
		{
			this.Build ();
//			eventbox.ButtonPressEvent += (o, args) => {
//				Console.WriteLine("cool");
			//			};
			this.SetSizeRequest (200, 200);
		}

		private void StartLoadAnimation ()
		{
			this.image.PixbufAnimation = loadAnimation;
		}

		private void StopLoadAnimation (byte[] data = null)
		{
			this.image.PixbufAnimation = null;
			if (data != null) {
				this.image.Pixbuf = new Pixbuf (data);
			}
		}

		public string Link {
			get {
				return link;
			}
			set {
				link = value;
			}
		}

		public string Label {
			get {
				return this.label.LabelProp;
			}
			set {
				this.label.LabelProp = String.Format ("<b>{0}</b>", value);
			}
		}

		public string Url {
			get {
				return url;
			}
			set {
				StartLoadAnimation ();
				RequestHelper.getImage (value, (byte[] data) => {
					Gtk.Application.Invoke (delegate {
						StopLoadAnimation (data);
						url = value;
					});
				}, (ex) => {
					if (OnLoadFailed != null) {
						OnLoadFailed (this);
					}
				});
			}
		}
	}
}

