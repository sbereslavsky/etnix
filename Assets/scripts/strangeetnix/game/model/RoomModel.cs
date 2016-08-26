using System;
using System.Collections.Generic;

namespace strangeetnix.game
{
	public class RoomModel : IRoomModel
	{
		//public IScoreModel scoreModel { get; set;}

		public int score { get; set; }

		public bool hasEnemy { get; private set; }

		public int enemyId { get; set; }
		public int enemyCount { get; set; }

		public EnemyTriggerManager enemyManager { get; private set; }

		public IBgAssetVO bgAssetInfo { get; set; }

		public IWaveVO waveVO { get; private set; }

		public List<int> enemyIdList { get; private set; }
		private List<IEnemyModel> _enemyModelList;

		public RoomModel ()
		{			
		}

		public void Reset ()
		{
			score = 0;
			enemyCount = 0;
			enemyId = 0;
		}

		public void setHasEnemy(bool value)
		{
			hasEnemy = value;
			if (hasEnemy) {
				enemyManager = new EnemyTriggerManager ();
			}
		}

		public void setWaveVO(IGameConfig gameConfig, IWaveVO waveVO1)
		{
			enemyIdList = new List<int> ();
			waveVO = waveVO1;
			if (waveVO != null) {
				enemyIdList = waveVO.enemy_encounter_id_list;
			}

			_enemyModelList = new List<IEnemyModel> ();
			if (enemyIdList != null) {
				IEnemyVO enemyVO;
				IEnemyModel enemyModel;
				int enemyId;
				for (byte i = 0; i < enemyIdList.Count; i++) {
					enemyId = enemyIdList [i];
					if (enemyId > gameConfig.assetConfig.enemyAssetList.Count) {
						enemyId = gameConfig.assetConfig.enemyAssetList.Count;
					}
					enemyVO = gameConfig.enemyConfig.getEnemyVOById (enemyId);
					if (enemyVO != null && getEnemyModelById(enemyId) == null) {
						enemyModel = new EnemyModel (enemyId, gameConfig);
						_enemyModelList.Add (enemyModel);
					}
				}

				if (enemyManager != null) {
					enemyManager.reset ();
				}
			}
		}

		public IEnemyModel getEnemyModelById(int id) 
		{
			for (byte i = 0; i < _enemyModelList.Count; i++) {
				if (_enemyModelList [i].id == id) {
					return _enemyModelList [i];
				}
			}

			return null;
		}
	}
}

