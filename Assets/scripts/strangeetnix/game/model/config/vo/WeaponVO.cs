using System;

namespace strangeetnix.game
{
	public class WeaponVO : ParseJSONObject, IWeaponVO
	{
		private string WEAPON_ID 	= "weaponid";
		private string OWNER_ID 	= "ownerid";
		private string ASSET_ID 	= "assetid";
		private string REQUIRE_STR 	= "requirestr";
		private string REQUIRE_DEX	= "requiredex";
		private string DAMAGE 		= "dmg";
		private string COOLDOWN 	= "cooldown";
		private string COST 		= "cost";
		private string INFO 		= "info";

		public int id { get; private set;}
		public int owner_id { get; private set;}
		public int asset_id { get; private set;}
		public int require_str { get; private set;}
		public int require_dex { get; private set;}
		public int damage { get; private set;}
		public int cooldown { get; private set;}
		public int cost { get; private set;}
		public string info { get; private set;}

		public WeaponVO (JSONObject value)
		{
			id = getInt32(value, WEAPON_ID);
			owner_id = getInt32(value, OWNER_ID);
			asset_id = getInt32(value, ASSET_ID);
			require_str = getInt32(value, REQUIRE_STR);
			require_dex = getInt32(value, REQUIRE_DEX);
			damage = getInt32(value, DAMAGE);
			cooldown = getInt32(value, COOLDOWN);
			cost = getInt32(value, COST);
			info = getString(value, INFO);
		}
	}
}