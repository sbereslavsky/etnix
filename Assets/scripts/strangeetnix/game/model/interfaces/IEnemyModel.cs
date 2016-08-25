using System;
using System.Collections.Generic;

namespace strangeetnix.game
{
	public interface IEnemyModel
	{
		int id { get; }
		float speed { get; }
		int damage { get; }
		int cooldown { get; }
		int hp { get; }
		int exp_give { get; }
		int gold_drop_min { get; }
		int gold_drop_max { get; }

		ICharAssetVO assetVO { get; }
	}
}