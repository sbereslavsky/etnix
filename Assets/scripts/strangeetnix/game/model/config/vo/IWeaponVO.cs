using System;

namespace strangeetnix.game
{
	public interface IWeaponVO
	{
		int id { get; }
		int owner_id { get; }
		int asset_id { get; }
		int require_str { get; }
		int require_dex { get; }
		int damage { get; }
		int cooldown { get; }
		int cost { get; }
		string info { get; }
	}
}