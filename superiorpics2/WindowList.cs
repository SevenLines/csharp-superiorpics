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

		protected int lastSelectedWidgetIndex;
		protected List<Button> widgets = new List<Button> ();

		public string ActiveItem {
			get { 
				if (widgets.Count > 0 && lastSelectedWidgetIndex < widgets.Count) {
					var text = widgets [lastSelectedWidgetIndex].Label; 
					return text.Replace (">> ", "");
				}
				return null;
			}
		}

		public List<string> Items {
			set {
				lastSelectedWidgetIndex = 0;
				widgets.Clear ();

				list.Foreach (delegate(Gtk.Widget widget) {
					list.Remove (widget);
				});

				foreach (var item in value) {
					var label = new Button (item);
					label.Show ();
					list.PackStart (label, false, false, 0);
					widgets.Add (label);

					label.Clicked += (object sender, EventArgs e) => {
						if (this.OnSelect != null) {
							this.OnSelect (label.Label);
						}
						this.Hide ();
					};
				}
				SetActive (0);
			}
		}

		public void SetActive(int index){
			if (widgets.Count <= 0)
				return;
			widgets [lastSelectedWidgetIndex].Label = 
				widgets [lastSelectedWidgetIndex].Label.Replace (">> ", "");
			widgets [index].Label = ">> " + widgets [index].Label;
			lastSelectedWidgetIndex = index;
		}

		public void SelectNext ()
		{
			if (widgets.Count <= 0)
				return;
			int index = (lastSelectedWidgetIndex + 1) % widgets.Count;
			SetActive (index);
		}

		public void SelectPrevious ()
		{
			if (widgets.Count <= 0)
				return;
			int index = (lastSelectedWidgetIndex - 1) % widgets.Count;
			SetActive (index);
		}
	}
}

