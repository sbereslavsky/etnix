using System;
using System.Collections.Generic;

namespace strangeetnix.game
{
	public interface IAssetConfig
	{
		Dictionary<int, ICharAssetVO> enemyAssetList { get; }

		ICharAssetVO getPlayerAssetById(int id);
		ICharAssetVO getEnemyAssetById (int id);
		IBgAssetVO getBgAssetById (int id);

		List<AssetPathData> villageAssetDataList { get; }
		List<AssetPathData> churchAssetDataList { get; }

		void initMainAssets (int playerId);
		void initGameAssets (List<int> enemyIds);
	}
}

