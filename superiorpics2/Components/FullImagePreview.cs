using System;

namespace superiorpics
{
	public partial class FullImagePreview : Gtk.Window
	{
		public FullImagePreview () :
			base (Gtk.WindowType.Toplevel)
		{
			this.Build ();
		}

		private void LoadImage ()
		{
			imageloader.StartLoadAnimation ();
			var hosting = new ImageBumHosting ();
			if (hosting.IsHostingFor (url)) {
				hosting.GetImageUrl (url, (image_src) => {
					imageloader.Url = image_src;
				});
			}
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

