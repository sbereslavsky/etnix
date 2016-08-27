using System;

namespace strangeetnix.game
{
	public class EquipedVO : ParseJSONObject, IEquipedVO
	{
		private string EQUIPED_ID	= "equipedid";
		private string AB1 			= "ab1";
		private string AB2 			= "ab2";
		private string AB3 			= "ab3";
		private string HP 			= "hp";
		private string STR 			= "str";
		private string DEX 			= "dex";
		private string COST 		= "cost";
		private string ASSET_ID 	= "assetid";
		private string INFO 		= "info";

		public int id { get; private set;}
		public int ab1 { get; private set;}
		public int ab2 { get; private set;}
		public int ab3 { get; private set;}
		public int hp { get; private set;}
		public int str { get; private set;}
		public int dex { get; private set;}
		public int cost { get; private set;}
		public int asset_id { get; private set;}
		public string info { get; private set;}
		
		public EquipedVO (JSONObject value)
		{
			id = getInt32(value, EQUIPED_ID);

			ab1 = getInt32(value, AB1);
			ab2 = getInt32(value, AB2);
			ab3 = getInt32(value, AB3);

			hp = getInt32(value, HP);
			dex = getInt32(value, DEX);
			str = getInt32(value, STR);

			asset_id = getInt32(value, ASSET_ID);
			cost = getInt32(value, COST);
			info = getString(value, INFO);
		}
	}
}