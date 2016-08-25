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
		public int gold_drop_min { get; private set;}
		public int gold_drop_max { get; private set;}

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

			gold_drop_min = enemyVO.gold_drop_min;
			gold_drop_max = enemyVO.gold_drop_max;
		}
	}
}