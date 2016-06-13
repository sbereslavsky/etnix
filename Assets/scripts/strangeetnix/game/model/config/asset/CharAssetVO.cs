using System;

namespace strangeetnix.game
{
	public class CharAssetVO : ICharAssetVO
	{
		public int id { get; private set; }
		public string name { get; private set; }
		public string path { get; private set; }
		public float delayToHit { get; internal set; }
		public float delayToDestroy { get; internal set; }
		public bool hasExplosion { get; internal set; }

		public CharAssetVO (int id1, string title1, string folder, float delayToHit1, float delayToDestroy1, bool hasExplosion1)
		{
			id = id1;
			name = title1;
			path = folder + name;
			delayToHit = delayToHit1;
			delayToDestroy = delayToDestroy1;
			hasExplosion = hasExplosion1;
		}
	}
}

