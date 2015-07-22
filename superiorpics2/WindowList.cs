using System;
using System.Collections.Generic;
using Gtk;


namespace superiorpics
{
	public partial class WindowList : Gtk.Window
	{
		public WindowList () :
			base (Gtk.WindowType.Popup)
		{
			this.Build ();

			this.Hide ();
		}

		public Action<String> OnSelect;

		public List<string> Items {
			set {
				list.Foreach (delegate(Gtk.Widget widget) {
					list.Remove (widget);
				});
				foreach (var item in value) {
					var label = new Button (item);
					label.Show ();
					list.PackStart (label);

					label.Clicked += (object sender, EventArgs e) => {
						if (this.OnSelect != null) {
							this.OnSelect (label.Label);
						}
						this.Hide();
					};
				}
			}
		}
	}
}

