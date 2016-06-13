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
		string drop_id_list { get; }
		string drop_rate_list { get; }
	}
}