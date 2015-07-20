using NUnit.Framework;
using System;
using superiorpics;
using HtmlAgilityPack;

namespace superiorpics
{
	[TestFixture ()]
	public class NUnitSuperiorPics
	{

		[Test ()]
		public void TestGalleryItemsGet ()
		{
			var source = new SuperiorpicsSource ();
			string data = RequestHelper.getRequest (@"http://forums.superiorpics.com/ubbthreads/ubbthreads.php/topics/4681213/Alison_Brie_2010_Scream_Awards");

			var doc = new HtmlDocument ();
			doc.LoadHtml (data);
			var root = doc.DocumentNode;

			var nodes = source.get_gallery_items_nodes (root);
			Assert.AreEqual (nodes.Count, 12);
		}
	}
}

