using NUnit.Framework;
using System;

namespace superiorpics
{
	[TestFixture ()]
	public class NUnitRadicalHosting
	{
		[Test ()]
		public void TestShouldCheckIsHostingFor ()
		{
			Assert.True (new RadicalHosting ().IsHostingFor ("http://radikal.ru/F/s41.radikal.ru/i093/0903/53/7731529eb418.jpg.html"));
			Assert.False (new RadicalHosting ().IsHostingFor ("http://radiskal.ru/F/s41.radikal.ru/i093/0903/53/7731529eb418.jpg.html"));
		}

		[Test ()]
		public async void TestShouldReturnCorrectUrl ()
		{
			var hosting = new RadicalHosting ();
			hosting.GetImageUrl ("http://radikal.ru/F/s41.radikal.ru/i093/0903/53/7731529eb418.jpg.html",
				(url) => {
					Assert.AreEqual (
						"http://s41.radikal.ru/i093/0903/53/7731529eb418.jpg",
						url);
				});
		}
	}
}

