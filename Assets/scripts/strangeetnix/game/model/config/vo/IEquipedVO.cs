using System;

namespace strangeetnix.game
{
	public interface IEquipedVO
	{
		int id { get; }

		int ab1 { get; }
		int ab2 { get; }
		int ab3 { get; }

		int hp { get; }
		int str { get; }
		int dex { get; }

		int cost { get; }
		int asset_id { get; }
		string info { get; }
	}
}