using System;

namespace strangeetnix.game
{
	public class AssetVO : IAssetVO
	{
		public int id { get; private set; }
		public AssetPathData assetData { get; private set; }

		public AssetVO (int id1, AssetPathData assetData1)
		{
			id = id1;
			assetData = assetData1;
		}
	}
}

