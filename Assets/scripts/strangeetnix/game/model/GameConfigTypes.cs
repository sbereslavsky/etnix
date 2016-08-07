using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace strangeetnix.game
{
	public static class GameConfigTypes
	{
		public const string CHAR_AB 		= "char_ab";
		public const string CHAR_ALL 		= "char_all";
		public const string CHAR_INFO 		= "char_info";

		public const string ENEMY 			= "enemy";
		public const string ITEMS 			= "items";
		public const string WAVES 			= "waves";
		public const string WEAPONS 		= "weapons";
		public const string EQUIPED 		= "equiped";

		public const string LOCALIZATION 	= "localisation";

		public const string USER_DATA	 	= "userData";

		public static List<string> list = new List<string> { CHAR_AB, CHAR_ALL, CHAR_INFO, ENEMY, ITEMS, WAVES, WEAPONS, EQUIPED, LOCALIZATION };
	}
}