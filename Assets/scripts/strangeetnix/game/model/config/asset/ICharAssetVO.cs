using System;

namespace strangeetnix.game
{
	public interface ICharAssetVO : IAssetVO
	{
		float delayToHit { get; }
		float delayToDestroy { get; }
		bool hasExplosion { get; }
	}
}

