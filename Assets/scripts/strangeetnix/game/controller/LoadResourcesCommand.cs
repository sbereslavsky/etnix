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
			List<AssetPathData> assetDataList = null;
			switch (preloaderType) {
			case PreloaderTypes.MAIN:
				gameConfig.assetConfig.initMainAssets (gameModel.playerId);
				assetDataList = gameConfig.assetConfig.villageAssetDataList;
				break;
			case PreloaderTypes.GAME:
				int waveId = gameModel.waveId;
				IWaveVO waveVO = gameConfig.waveConfig.getWaveVOById (waveId);

				gameConfig.assetConfig.initGameAssets (waveVO.enemy_unique_id_list);
				assetDataList = gameConfig.assetConfig.churchAssetDataList;
				break;

			}

			if (assetDataList != null) {
				for (int i = 0; i < assetDataList.Count; i++) {
					resourceManager.addAssetDataToLoad (assetDataList [i]);
				}

				resourceManager.initRequests (preloaderType);
			}

			if (resourceManager.resourceLoadCount > 0) {
				addPreloaderSignal.Dispatch (preloaderType);
				resourceManager.startLoad ();
			} else {
				resourceManager.callbackAfterLoad ();
			}
		}
	}
}

