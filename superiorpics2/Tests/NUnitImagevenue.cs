using NUnit.Framework;
using System;
using System.Text.RegularExpressions;

namespace superiorpics
{
	[TestFixture ()]
	public class NUnitImagevenueHosting
	{
		[Test ()]
		public void TestShouldCheckIsHostingFor ()
		{
			var hosting = new ImagevenueHosting ();
			Assert.True (hosting.IsHostingFor (
				@"http://img45.imagevenue.com/img.php?image=65833_Kate_Beckinsale___2008_12_02_Nothing_But_the_Truth_Portraits_by_Leo_Rigah1_122_10lo.jpg"
			));
			Assert.False (hosting.IsHostingFor (
				@"http://img45.aimagevenue.com/img.php?image=65833_Kate_Beckinsale___2008_12_02_Nothing_But_the_Truth_Portraits_by_Leo_Rigah1_122_10lo.jpg"
			));
		}

		[Test()]
		public async void TestShouldGetCorrectImageUrl()
		{
			var hosting = new ImagevenueHosting ();
			hosting.GetImageUrl ("http://img45.imagevenue.com/img.php?image=65833_Kate_Beckinsale___2008_12_02_Nothing_But_the_Truth_Portraits_by_Leo_Rigah1_122_10lo.jpg",
				(url) => {
					Assert.True (Regex.IsMatch(url,
						@"http://img45\.imagevenue\.com/aAfkjfp01fo1i-(\d+)/loc10/65833_Kate_Beckinsale___2008_12_02_Nothing_But_the_Truth_Portraits_by_Leo_Rigah1_122_10lo\.jpg"
					));
				});
		}
	}
}

