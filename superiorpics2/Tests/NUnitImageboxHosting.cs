using NUnit.Framework;
using System;

namespace superiorpics
{
	[TestFixture ()]
	public class NUnitImageboxHosting
	{
		[Test ()]
		public void TestShouldCheckIsHostingFor ()
		{
			Assert.True (new ImageboxHosting ().IsHostingFor ("http://imgbox.com/UI4rlhD8"));
			Assert.False (new ImageboxHosting ().IsHostingFor ("http://imgb2ox.com/UI4rlhD8"));
		}

		[Test ()]
		public async void TestShouldReturnCorrectUrl ()
		{
			var hosting = new ImageboxHosting ();
			hosting.GetImageUrl ("http://imgbox.com/UI4rlhD8",
				(url) => {
					Assert.AreEqual (
						"http://i.imgbox.com/UI4rlhD8.jpg",
						url);
				});
		}
	}
}

