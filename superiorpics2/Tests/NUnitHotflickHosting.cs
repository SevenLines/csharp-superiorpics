using NUnit.Framework;
using System;

namespace superiorpics
{
	[TestFixture ()]
	public class NUnitHotflickHosting
	{
		[Test ()]
		public void TestShouldCheckIsHostingFor ()
		{
			Assert.True (new HotflickHosting ().IsHostingFor ("http://www.hotflick.net/u/v/?q=1242313.th_41839_8gaffm122__122_1191lo.jpg"));
			Assert.False (new HotflickHosting ().IsHostingFor ("http://www.hotfli2ck.net/u/v/?q=1242313.th_41839_8gaffm122__122_1191lo.jpg"));
		}

		[Test ()]
		public async void TestShouldReturnCorrectUrl ()
		{
			var hosting = new HotflickHosting ();
			hosting.GetImageUrl ("http://www.hotflick.net/u/v/?q=1242313.th_41839_8gaffm122__122_1191lo.jpg",
				(url) => {
					Assert.AreEqual (
						"http://www.hotflick.net/u/n/1242313/th_41839_8gaffm122__122_1191lo.jpg",
						url);
				});
		}
	}
}

