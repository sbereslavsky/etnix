using System;
using strange.extensions.command.impl;
using UnityEngine;

namespace strangeetnix.game
{
	public class CreateEnemyWaveCommand : Command
	{
		[Inject]
		public IGameConfig gameConfig{ get; set; }

		[Inject]
		public IGameModel gameModel{ get; set; }

		[Inject]
		public int waveId{ get; set; }

		public override void Execute ()
		{
			IWaveVO waveVO = gameConfig.waveConfig.getWaveVOById (waveId);
			if (waveVO != null) {
				gameModel.roomModel.setWaveVO (gameConfig, waveVO);
			} else {
				Debug.LogError ("This is not wave id = " + waveId + " !!!");
			}
		}
	}
}

