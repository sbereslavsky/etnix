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
		List<int> drop_id_list { get; }
		List<int> drop_rate_list { get; }

		ICharAssetVO assetVO { get; }
	}
}