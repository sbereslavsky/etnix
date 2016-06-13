using System;

namespace strangeetnix.game
{
	public class AssetVO : IAssetVO
	{
		public int id { get; private set; }
		public string name { get; private set; }
		public string path { get; private set; }

		public AssetVO (int id1, string title1, string folder)
		{
			id = id1;
			name = title1;
			path = folder + name;
		}
	}
}

