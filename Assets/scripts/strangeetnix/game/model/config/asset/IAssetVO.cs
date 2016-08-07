using System;

namespace strangeetnix.game
{
	public interface IAssetVO
	{
		int id { get; }
		AssetPathData assetData { get; }
	}
}

