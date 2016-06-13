using System;
using UnityEngine;
using strange.extensions.command.impl;
using strangeetnix.ui;

namespace strangeetnix.game
{
	public class ResetGameCameraCommand : Command
	{
		[Inject]
		public IGameModel gameModel{ get; set; }

		[Inject]
		public bool disable{ get; set; }

		public override void Execute ()
		{
			GameObject gameCamera = GameObject.Find ("gameCamera");
			if (gameCamera) {
				GameCameraView cameraView = gameCamera.GetComponent<GameCameraView> ();
				if (cameraView != null) {
					IBgAssetVO bgAssetVO = gameModel.levelModel.bgAssetInfo;
					cameraView.xMargin = bgAssetVO.margin.x;
					cameraView.yMargin = bgAssetVO.margin.y;
					cameraView.xSmooth = bgAssetVO.smooth.x;
					cameraView.ySmooth = bgAssetVO.smooth.x;
					cameraView.minXAndY = bgAssetVO.minXAndY;
					cameraView.maxXAndY = bgAssetVO.maxXAndY;
					cameraView.updateObjects ();
					cameraView.updateCamera (gameModel.playerPosX, disable);
					if (disable) {
						cameraView.enabled = false;
					}
				}
			}
		}
	}
}

