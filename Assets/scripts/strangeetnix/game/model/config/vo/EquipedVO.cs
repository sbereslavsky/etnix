using System;

namespace strangeetnix.game
{
	public class EquipedVO : ParseJSONObject, IEquipedVO
	{
		private string EQUIPED_ID	= "equipedid";
		private string AB1 			= "ab1";
		private string COST 		= "cost";
		private string ASSET_ID 	= "assetid";
		private string INFO 		= "info";

		public int id { get; private set;}
		public int ab1 { get; private set;}
		public int cost { get; private set;}
		public int asset_id { get; private set;}
		public string info { get; private set;}
		
		public EquipedVO (JSONObject value)
		{
			id = getInt32(value, EQUIPED_ID);
			ab1 = getInt32(value, AB1);
			asset_id = getInt32(value, ASSET_ID);
			cost = getInt32(value, COST);
			info = getString(value, INFO);
		}
	}
}