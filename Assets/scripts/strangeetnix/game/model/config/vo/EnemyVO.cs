using System;

namespace strangeetnix.game
{
	public class EnemyVO : ParseJSONObject, IEnemyVO
	{
		private string ENEMY_ID 		= "enemyid";
		private string ASSET_ID 		= "assetid";
		private string SPEED 			= "speed";
		private string DAMAGE 			= "damage";
		private string COOLDOWN			= "cooldown";
		private string HP 				= "hp";
		private string EXP_GIVE 		= "expgive";
		private string DROP_ID_LIST 	= "dropidlist";
		private string DROP_RATE_LIST 	= "dropratelist";

		public int id { get; private set;}
		public int asset_id { get; private set;}
		public float speed { get; private set;}
		public int damage { get; private set;}
		public int cooldown { get; private set;}
		public int hp { get; private set;}
		public int exp_give { get; private set;}
		public string drop_id_list { get; private set;}
		public string drop_rate_list { get; private set;}

		public EnemyVO (JSONObject value)
		{
			id = getInt32(value, ENEMY_ID);
			asset_id = getInt32(value, ASSET_ID);
			speed = getFloat(value, SPEED);
			damage = getInt32(value, DAMAGE);
			cooldown = getInt32(value, COOLDOWN);
			hp = getInt32(value, HP);
			exp_give = getInt32(value, EXP_GIVE);
			drop_id_list = getString(value, DROP_ID_LIST);
			drop_rate_list = getString(value, DROP_RATE_LIST);
		}
	}
}