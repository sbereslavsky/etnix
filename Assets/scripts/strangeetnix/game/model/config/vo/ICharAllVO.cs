using System;

namespace strangeetnix.game
{
	public interface ICharAllVO
	{
		int level_id { get; }
		int exp_next { get; }

		int ab1_next { get; }
		int ab2_next { get; }
		int ab3_next { get; }

		int ch1_hp { get; }
		int ch1_str { get; }
		int ch1_dex { get; }

		int ch2_hp { get; }
		int ch2_str { get; }
		int ch2_dex { get; }

		int exp_help { get; }
	}
}