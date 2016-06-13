using System;

namespace strangeetnix.game
{
	public class CharInfoVO : ParseJSONObject, ICharInfoVO
	{
		static public string ID = "id";
		static public string NAME = "name";
		static public string SPEED = "speed";

		public int id { get; private set;}
		public string name { get; private set;}
		public float speed { get; private set;}

		public CharInfoVO (JSONObject value)
		{
			id = getInt32 (value, ID);
			name = getString (value, NAME);
			speed = getFloat (value, SPEED);
		}
	}
}