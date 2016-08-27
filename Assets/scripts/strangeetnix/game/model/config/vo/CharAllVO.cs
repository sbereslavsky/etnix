using System;

namespace strangeetnix.game
{
	public class CharAllVO : ParseJSONObject, ICharAllVO
	{
		private string LVL_ID   = "lvl";
		private string EXP_NEXT = "expnext";
		private string AB1_NEXT = "ab1next";
		private string AB2_NEXT = "ab2next";
		private string AB3_NEXT = "ab3next";
		private string CH1_HP   = "ch1hp";
		private string CH1_STR  = "ch1str";
		private string CH1_DEX  = "ch1dex";
		private string CH2_HP   = "ch2hp";
		private string CH2_STR  = "ch2str";
		private string CH2_DEX  = "ch2dex";
		private string EXP_HELP  = "exphelp";

		public int level_id { get; private set;}
		public int exp_next { get; private set;}

		public int ab1_next { get; private set;}
		public int ab2_next { get; private set;}
		public int ab3_next { get; private set;}

		public int ch1_hp { get; private set;}
		public int ch1_str { get; private set;}
		public int ch1_dex { get; private set;}

		public int ch2_hp { get; private set;}
		public int ch2_str { get; private set;}
		public int ch2_dex { get; private set;}

		public int exp_help { get; private set;}

		public CharAllVO (JSONObject value)
		{
			level_id = getInt32(value, LVL_ID);
			exp_next = getInt32(value, EXP_NEXT);

			ab1_next = getInt32(value, AB1_NEXT);
			ab2_next = getInt32(value, AB2_NEXT);
			ab3_next = getInt32(value, AB3_NEXT);

			ch1_hp = getInt32(value, CH1_HP);
			ch1_str = getInt32(value, CH1_STR);
			ch1_dex = getInt32(value, CH1_DEX);

			ch2_hp = getInt32(value, CH2_HP);
			ch2_str = getInt32(value, CH2_STR);
			ch2_dex = getInt32(value, CH2_DEX);

			exp_help = getInt32 (value, EXP_HELP);
		}
	}
}