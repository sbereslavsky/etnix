using System;
using System.Linq;
using System.Collections.Generic;
using strange.extensions.command.impl;
using UnityEngine;
using strange.extensions.pool.api;

using strangeetnix.ui;

namespace strangeetnix.game
{
	public class LoadResourcesCommand : Command
	{
		[Inject]
		public IGameConfig gameConfig{ get; set; }

		[Inject]
		public IGameModel gameModel{ get; set; }

		[Inject]
		public IResourceManager resourceManager{ get; set; }

		[Inject]
		public AddPreloaderSignal addPreloaderSignal{ get; set; }

		[Inject]
		public PreloaderTypes preloaderType{ get; set; }

		public override void Execute()
		{
			addPreloaderSignal.Dispatch (preloaderType);

			if (!gameModel.levelModel.hasEnemy) {
				gameConfig.assetConfig.initMainAssets (gameModel.playerId);
			}
			else {
				int waveId = gameModel.waveId;
				IWaveVO waveVO = gameConfig.waveConfig.getWaveVOById (waveId);

				gameConfig.assetConfig.initGameAssets (waveVO.enemy_unique_id_list);
			}

			List<AssetPathData> assetDataList = (gameModel.levelModel.hasEnemy) ? gameConfig.assetConfig.churchAssetDataList : gameConfig.assetConfig.villageAssetDataList;
			for (int i = 0; i < assetDataList.Count; i++) {
				resourceManager.addResorceToLoad (assetDataList [i]);
			}

			resourceManager.startLoad (preloaderType);
		}
	}
}

