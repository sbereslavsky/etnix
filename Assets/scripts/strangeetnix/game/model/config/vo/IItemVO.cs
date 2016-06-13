using System;

namespace strangeetnix.game
{
	public interface IItemVO
	{
		int id { get; }
		string type { get; }
		string interactive_id { get; }
		int action_str { get; }
		int asset_id { get; }
		int cost { get; }
		string info { get; }
	}
}
