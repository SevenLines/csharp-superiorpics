using System;
using System.Collections.Generic;
using System.Linq;

namespace superiorpics
{
	[System.ComponentModel.ToolboxItem (true)]
	public partial class RandomCelebs : Gtk.Bin
	{
		public Action<RandomCelebs> OnRandomButtonClick;
		public Action<CelebrityItemJson> OnItemClick;
		public Dictionary<ImageLoader, CelebrityItemJson> imageLoaders = new Dictionary<ImageLoader, CelebrityItemJson> ();

		public RandomCelebs ()
		{
			this.Build ();
		}

		public int ItemsCount {
			get {
				return imageLoaders.Count;
			}
			set {
				foreach (var image in imageLoaders.Keys) {
					this.vbox1.Remove (image);
				}

				imageLoaders.Clear ();
				for (int i = 0; i < value; ++i) {
					var imageLoader = new ImageLoader ();
					imageLoader.Label = "";
					imageLoader.ShowButtons = false;
					imageLoader.Show ();
					imageLoaders.Add (imageLoader, null);
					imageLoader.ButtonPressEvent += (o, args) => {
						if (OnItemClick != null) {
							OnItemClick (imageLoaders [imageLoader]);
						}
					};
					this.vbox1.Add (imageLoader);
				}
			}
		}

		public void GenRandom (List<CelebrityItemJson> celebs)
		{
			var r = new Random ();

			for (var i = 0; i < imageLoaders.Keys.Count; ++i) {
				ImageLoader imageLoader = imageLoaders.Keys.ToList () [i];
				var celeb = celebs.ElementAt (r.Next (celebs.Count));
				imageLoader.ShowButtons = false;
				imageLoader.Label = celeb.Name;
				imageLoader.Url = celeb.Thumb;
				imageLoaders [imageLoader] = celeb;
			}
		}

		protected void OnBtnRandomClicked (object sender, EventArgs e)
		{
			if (OnRandomButtonClick != null) {
				OnRandomButtonClick (this);
			}
		}
	}
}

