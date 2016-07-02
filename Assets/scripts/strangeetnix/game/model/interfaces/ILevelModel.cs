using System;
using System.Collections.Generic;

namespace strangeetnix.game
{
	public interface ILevelModel
	{
		//IScoreModel scoreModel { get; set;}
		int score { get; set; }

		bool hasEnemy { get; }

		int enemyCount { get; set; }

		//maybe use only enemyTriggerManager
		PlayerTriggerManager playerTriggerManager { get; } 

		EnemyManager enemyManager { get; }

		IBgAssetVO bgAssetInfo { get; set; }

		IWaveVO waveVO { get; }

		List<int> enemyIdList { get; }

		void setWaveVO (IGameConfig gameConfig, IWaveVO waveVO);

		IEnemyModel getEnemyModelById (int id);

		void Reset ();
		void setHasEnemy (bool value);
	}
}

