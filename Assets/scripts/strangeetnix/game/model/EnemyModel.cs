using System;
using System.Linq;
using System.Collections.Generic;

namespace strangeetnix.game
{
	public class EnemyModel : IEnemyModel 
	{
		public int id { get; private set;}
		public float speed { get; private set;}
		public int damage { get; private set;}
		public int cooldown { get; private set;}
		public int hp { get; private set;}
		public int exp_give { get; private set;}
		public List<int> drop_id_list { get; private set;}
		public List<int> drop_rate_list { get; private set;}

		public ICharAssetVO assetVO { get; private set; }

		public EnemyModel(int setId, IGameConfig gameConfig)
		{
			id = setId;

			parseData (gameConfig);
		}

		private void parseData(IGameConfig gameConfig)
		{
			IEnemyVO enemyVO = gameConfig.enemyConfig.getEnemyVOById (id);
			assetVO = gameConfig.assetConfig.getEnemyAssetById (enemyVO.asset_id);

			speed = enemyVO.speed;
			hp = enemyVO.hp;
			damage = enemyVO.damage;
			cooldown = enemyVO.cooldown;
			exp_give = enemyVO.exp_give;

			drop_id_list = convertStringToList (enemyVO.drop_id_list);
			drop_rate_list = convertStringToList (enemyVO.drop_rate_list);
		}

		private List<int> convertStringToList(string value)
		{
			List<int> result = new List<int> ();
			if (value != null && value.Length > 0) {
				string[] ids = value.Split (',');
				for (byte i = 0; i < ids.Length; i++) {
					result.Add (Convert.ToInt32(ids.GetValue(i)));
				}
			}
			return result;
		}
	}
}