using System;
using System.Collections.Generic;

namespace superiorpics
{
	[System.ComponentModel.ToolboxItem (true)]
	public partial class ResponsiveGrid : Gtk.Bin
	{
		private List<Gtk.Widget> widgets = new List<Gtk.Widget> ();
		private Gtk.Table table = new Gtk.Table (1, 1, false);

		public ResponsiveGrid ()
		{
			this.Build ();
			Add (table);
			table.Show ();
		}


		public void Rebuild ()
		{
			if (widgets.Count == 0) {
				return;
			}

			table.NColumns = 3;
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

