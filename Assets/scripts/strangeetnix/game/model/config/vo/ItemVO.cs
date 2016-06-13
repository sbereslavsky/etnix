using System;

namespace strangeetnix.game
{
	public class ItemVO : ParseJSONObject, IItemVO
	{
		private string ITEM_ID = "itemid";
		private string TYPE = "type";
		private string INTERACTIVE_ID = "interactiveid";
		private string ACTION_STR = "actionstr";
		private string ASSET_ID = "assetid";
		private string COST = "cost";
		private string INFO = "info";

		public int id { get; private set;}
		public string type { get; private set;}
		public string interactive_id { get; private set;}
		public int action_str { get; private set;}
		public int asset_id { get; private set;}
		public int cost { get; private set;}
		public string info { get; private set;}

		public ItemVO (JSONObject value)
		{
			id = getInt32(value, ITEM_ID);
			type = getString(value, TYPE);
			interactive_id = getString(value, INTERACTIVE_ID);
			action_str = getInt32(value, ACTION_STR);
			asset_id = getInt32(value, ASSET_ID);
			cost = getInt32(value, COST);
			info = getString(value, INFO);
		}
	}
}