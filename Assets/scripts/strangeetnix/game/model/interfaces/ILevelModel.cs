using System;
using System.Collections.Generic;

namespace strangeetnix.game
{
	public interface ILevelModel
	{
		//IScoreModel scoreModel { get; set;}
		int score { get; set; }

		bool hasEnemy { get; }

		int enemyId { get; set; }
		int enemyCount { get; set; }

		EnemyTriggerManager enemyManager { get; }

		IBgAssetVO bgAssetInfo { get; set; }

		IWaveVO waveVO { get; }

		List<int> enemyIdList { get; }

		void setWaveVO (IGameConfig gameConfig, IWaveVO waveVO);

		IEnemyModel getEnemyModelById (int id);

		void Reset ();
		void setHasEnemy (bool value);
	}
}

