using System;

namespace strangeetnix.game
{
	public interface IEquipedVO
	{
		int id { get; }
		int ab1 { get; }
		int cost { get; }
		int asset_id { get; }
		string info { get; }
	}
}