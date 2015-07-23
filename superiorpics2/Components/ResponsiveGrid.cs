using System;
using System.Collections.Generic;

namespace superiorpics
{
	[System.ComponentModel.ToolboxItem (true)]
	public partial class ResponsiveGrid : Gtk.Bin
	{
		private List<Gtk.Widget> widgets = new List<Gtk.Widget> ();
		private int maxSize = 210;


		public ResponsiveGrid ()
		{
			this.Build ();
		}

		public void Reallocate (int width)
		{
			var cols = Convert.ToUInt32 (width / maxSize);
			if (cols != table.NColumns) {
				RemoveAll (false);
				this.Rebuild (width);
			}
		}

		public void Rebuild (int allocation_width = -1)
		{
			if (this.widgets.Count == 0) {
				return;
			}

			if (allocation_width == -1) {
				allocation_width = this.Allocation.Width;
			}

			table.NColumns = Convert.ToUInt32 (allocation_width / maxSize);
			table.NRows = (uint)Math.Ceiling ((double)(widgets.Count) / table.NColumns);

			uint x = 0;
			uint y = 0;

			foreach (var widget in widgets) {
				if (widget.Parent == null) {
					table.Attach (widget, x, x + 1, y, y + 1);
					x = x + 1;
					if (x >= table.NColumns) {
						x = 0;
						y += 1;
					}
				}
			}
			table.Show ();
		}

		public void AddWidget (Gtk.Widget widget, bool forceRebuild = false)
		{
			widgets.Add (widget);
			if (forceRebuild)
				Rebuild ();
		}

		public void RemoveWidget (Gtk.Widget widget, bool forceRebuild = false)
		{
			if (forceRebuild) {
				widgets.ForEach ((w) => {
					if (w.Parent == this.table) {
						table.Remove (w);
					}
				});
			}

			widgets.Remove (widget);
			if (forceRebuild)
				Rebuild ();
		}

		public void RemoveAll (bool clear = true)
		{
			widgets.ForEach ((widget) => {
				if (widget.Parent == this.table) {
					table.Remove (widget);
				}
			});

			if (clear) {
				widgets.Clear ();
			}
		}

		public object this [int index] {
			get {
				return widgets [index];
			}
		}
	}
}

