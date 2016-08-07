using System;

namespace strangeetnix.game
{
	public class CharAssetVO : ICharAssetVO
	{
		public int id { get; private set; }
		public AssetPathData assetData { get; private set; }
		public float delayToHit { get; internal set; }
		public float delayToDestroy { get; internal set; }
		public bool hasExplosion { get; internal set; }

		public CharAssetVO (int id1, AssetPathData assetData1, float delayToHit1, float delayToDestroy1, bool hasExplosion1)
		{
			id = id1;
			assetData = assetData1;
			delayToHit = delayToHit1;
			delayToDestroy = delayToDestroy1;
			hasExplosion = hasExplosion1;
		}
	}
}

