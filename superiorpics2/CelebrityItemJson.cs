using System;
using Newtonsoft.Json;

namespace superiorpics
{
	public class CelebrityItemJson
	{
		[JsonProperty ("name")]
		public string Name {
			get;
			set;
		}

		[JsonProperty ("link")]
		public string Link {
			get;
			set;
		}

		[JsonProperty ("thumb")]
		public string Thumb {
			get;
			set;
		}
	}
}

