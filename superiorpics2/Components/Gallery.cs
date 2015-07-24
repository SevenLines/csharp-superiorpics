using System;
using Gtk;
using System.Collections.Generic;

namespace superiorpics
{
	[System.ComponentModel.ToolboxItem (true)]
	public partial class Gallery : Gtk.Bin
	{
		public Gallery ()
		{
			this.Build ();

			var renderer = new CellRendererText ();

			cmbPage.PackStart (renderer, false);
			cmbPage.AddAttribute (renderer, "text", 1);

			cmbPage.WrapWidth = 10;
		}

		public ListStore PagesModel {
			get {
				return (ListStore)cmbPage.Model;
			}
			set {
				cmbPage.Model = value;
			}
		}

		public Action<ForumItem> OnForumClick;
		public Action<GalleryItem> OnGalleryItemClick;
		public Action<int, string> OnPageChanged;
		public Action<string, ImageLoader> OnSaveClick;

		public void SaveAll ()
		{
			foreach (var widget in grid.Widgets) {
				var imageLoader = (ImageLoader)widget;
				imageLoader.PerformSaveClick ();
			}
		}

		public void SetForums (List<ForumItem> forums)
		{
			grid.RemoveAll ();
			foreach (var forum in forums) {
				var image = new ImageLoader ();
				image.Url = forum.thumb;
				image.Label = forum.title;
				image.Link = forum.url;
				image.ButtonPressEvent += (o, args) => {
					if (OnForumClick != null) {
						OnForumClick (forum);
					}
				};
				image.OnLoadFailed += (ImageLoader obj) => {
					Gtk.Application.Invoke (delegate {
						grid.RemoveWidget (obj, true);
					});
				};

				grid.AddWidget (image);
				image.Show ();
			}
			grid.Rebuild ();
		}

		public void SetGallery (List<GalleryItem> gallery)
		{
			grid.RemoveAll ();
			foreach (var item in gallery) {
				var image = new ImageLoader (true);
				image.Url = item.thumb;
				image.Link = item.url;
				image.ButtonPressEvent += (o, args) => {
					if (OnGalleryItemClick != null) {
						OnGalleryItemClick (item);
					}
				};
				image.OnLoadFailed += (ImageLoader obj) => {
					Gtk.Application.Invoke (delegate {
						grid.RemoveWidget (obj, true);
					});
				};
				image.OnSaveClick = (string link) => {
					if (OnSaveClick != null) {
						OnSaveClick (link, image);
					}
				};
				grid.AddWidget (image);
				image.Show ();
			}
			grid.Rebuild ();
		}

		protected void OnCmbPageChanged (object sender, EventArgs e)
		{
			if (OnPageChanged != null) {
				OnPageChanged (cmbPage.Active, cmbPage.ActiveText);
			}
		}

		protected void OnScrolledwindow2SizeAllocated (object o, SizeAllocatedArgs args)
		{
			grid.Reallocate (args.Allocation.Width);
		}
	}
}

