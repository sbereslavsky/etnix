using System;

namespace strangeetnix.game
{
	public interface IEnemyVO
	{
		int id { get; }
		int asset_id { get; }
		float speed { get; }
		int damage { get; }
		int cooldown { get; }
		int hp { get; }
		int exp_give { get; }
		int gold_drop_min { get; }
		int gold_drop_max { get; }
		string description { get; }
	}
}