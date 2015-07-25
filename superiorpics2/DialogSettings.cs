using System;

namespace superiorpics
{
	public partial class DialogSettings : Gtk.Dialog
	{
		public DialogSettings ()
		{
			this.Build ();
		}


		private FormSettings settings;

		public FormSettings Settings {
			set { settings = value; }
		}

		public string SaveDir {
			get { return this.saveDir.CurrentFolder; }
		}

		protected override void OnShown ()
		{
			if (settings != null) {
				this.saveDir.SetCurrentFolder (settings.SaveDir);
			}
			base.OnShown ();
		}

		protected override void OnResponse (Gtk.ResponseType response_id)
		{
			Hide ();
			base.OnResponse (response_id);
		}
	}
}

