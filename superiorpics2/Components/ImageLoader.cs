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
		public Action<string> OnSaveClick;
		public Pixbuf pixbuf;

		public ImageLoader (bool withButtons = false)
		{
			this.Build ();
//			eventbox.ButtonPressEvent += (o, args) => {
//				Console.WriteLine("cool");
			//			};
			this.SetSizeRequest (200, 200);
			if (!withButtons) {
				buttons.Hide ();
			}
		}

		public bool ShowButtons {
			get {
				return buttons.Visible;
			}
			set {
				if (value)
					buttons.Show ();
				else
					buttons.Hide ();
			}
		}

		public void StartLoadAnimation ()
		{
			this.image.Pixbuf = null;
			this.image.PixbufAnimation = loadAnimation;
		}

		public void StopLoadAnimation (byte[] data = null)
		{
			this.image.PixbufAnimation = null;
			if (data != null) {
				pixbuf = new Pixbuf (data);
				this.image.Pixbuf = pixbuf;
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

		public Gtk.Image Image {
			get  { return this.image; }
		}
		//
		//		public string Label {
		//			get {
		//				return this.label.LabelProp;
		//			}
		//			set {
		//				this.label.LabelProp = String.Format ("<b>{0}</b>", value);
		//			}
		//		}

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

		protected void OnBtnSaveClicked (object sender, EventArgs e)
		{
			if (OnSaveClick != null) {
				OnSaveClick (link);
			}
		}
	}
}

