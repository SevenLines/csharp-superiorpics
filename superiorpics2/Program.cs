using System;
using Gtk;

namespace superiorpics
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			// load log4net configurations from app.config
			log4net.Config.XmlConfigurator.Configure ();

			Application.Init ();
			MainWindow win = new MainWindow ();
			win.Show ();
			Application.Run ();
		}
	}
}
