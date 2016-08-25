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
		private string GOLD_DROP_MIN 	= "golddropmin";
		private string GOLD_DROP_MAX 	= "golddropmax";
		private string DESCRIPTION		= "description";

		public int id { get; private set;}
		public int asset_id { get; private set;}
		public float speed { get; private set;}
		public int damage { get; private set;}
		public int cooldown { get; private set;}
		public int hp { get; private set;}
		public int exp_give { get; private set;}
		public int gold_drop_min { get; private set;}
		public int gold_drop_max { get; private set;}
		public string description { get; private set;}

		public EnemyVO (JSONObject value)
		{
			id = getInt32(value, ENEMY_ID);
			asset_id = getInt32(value, ASSET_ID);
			speed = getFloat(value, SPEED);
			damage = getInt32(value, DAMAGE);
			cooldown = getInt32(value, COOLDOWN);
			hp = getInt32(value, HP);
			exp_give = getInt32(value, EXP_GIVE);
			gold_drop_min = getInt32(value, GOLD_DROP_MIN);
			gold_drop_max = getInt32(value, GOLD_DROP_MAX);
			description = getString (value, DESCRIPTION);
		}
	}
}