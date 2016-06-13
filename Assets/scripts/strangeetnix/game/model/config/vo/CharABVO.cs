using System;

namespace strangeetnix.game
{
	public class CharABVO : ParseJSONObject, ICharABVO
	{
		private string CHAR_ID = "charid";
		private string AB_ID = "abid";
		private string AB_LEVEL = "ablevel";

		public int char_id { get; private set;}
		public int ab_id { get; private set;}
		public int ab_level { get; private set;}

		public CharABVO (JSONObject value)
		{
			char_id = getInt32(value, CHAR_ID);
			ab_id = getInt32(value, AB_ID);
			ab_level = getInt32(value, AB_LEVEL);
		}
	}
}