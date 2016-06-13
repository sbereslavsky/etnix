using System;
using System.Collections.Generic;

namespace strangeetnix.game
{
	public interface IAssetConfig
	{
		List<ICharAssetVO> enemyAssetList { get; }

		ICharAssetVO getPlayerAssetById(int id);
		ICharAssetVO getEnemyAssetById (int id);
		IBgAssetVO getBgAssetById (int id);
	}
}

