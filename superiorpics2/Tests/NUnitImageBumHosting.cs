using NUnit.Framework;
using System;

namespace superiorpics
{
	[TestFixture ()]
	public class NUnitImageBumHosting
	{
		[Test ()]
		public void TestShouldCheckIsHostingFor ()
		{
			Assert.True (new ImageBumHosting ().IsHostingFor ("http://www.imagebam.com/image/bbc345419520843"));
			Assert.False (new ImageBumHosting ().IsHostingFor ("https://www.imagebam.com/image/bbc345419520843"));
		}

		[Test ()]
		public async void TestShouldReturnCorrectUrl ()
		{
			var hosting = new ImageBumHosting ();
			hosting.GetImageUrl ("http://www.imagebam.com/image/bbc345419520843",
				(url) => {
					Assert.AreEqual (
						"http://108.imagebam.com/download/6SDQEvv7IsKdl04-92aEUQ/41953/419520843/alisonbrieweb001.jpg",
						url);
				});
		}
	}
}

