using System;
using Gdk;

namespace superiorpics
{
	[System.ComponentModel.ToolboxItem (true)]
	public class ImageEx : Gtk.Image
	{
		Pixbuf pixbuf;

		public new Pixbuf Pixbuf {
			get {
				return pixbuf;
			}
			set {
				pixbuf = value;
				base.Pixbuf = pixbuf;
			}
		}

		protected override void OnSizeAllocated (Rectangle allocation)
		{
			base.OnSizeAllocated (allocation);
			if (pixbuf != null) {
				int newHeight, newWidth;
				double kPixbuf = (double)pixbuf.Height / pixbuf.Width;
				double kAllocation = (double)allocation.Height / allocation.Width;

				if (kPixbuf < kAllocation) {
					newWidth = allocation.Width;
					newHeight = (int)(newWidth * kPixbuf);
				} else {
					newHeight = allocation.Height;
					newWidth = (int)(newHeight / kPixbuf);
				}

				if (base.Pixbuf != null && base.Pixbuf.Height == newHeight && base.Pixbuf.Width == newWidth) {
					return;
				}

				base.Pixbuf = pixbuf.ScaleSimple (newWidth, newHeight, InterpType.Bilinear);
			}
		}
	}
}

