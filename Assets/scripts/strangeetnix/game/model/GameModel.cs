//Storage of data that reflects how the player is doing

using System;
using UnityEngine;
using System.Collections.Generic;

namespace strangeetnix.game
{
	public class GameModel : IGameModel
	{
		public GameModel ()
		{
		}

		#region IGameModel implementation

		private int ROOM_LEVEL_TYPE = 1000;

		public void Reset ()
		{
			levelId = 1;
			playerId = 0;
			waveId = 0;
			createEnemyId = 0;
			resetPlayerPosX ();
			roomNum = 0;
			if (levelModel == null) {
				levelModel = new LevelModel ();
			}

			levelModel.Reset ();

			if (playerModel != null) {
				playerModel.resetHp ();
			}
		}

		public void resetPlayerPosX ()
		{
			playerPosX = 4;
		}

		public int playerId { get; set; }
		public int createEnemyId { get; set; }
		public int levelId { get; set; }
		public int waveId { get; set; }
		public int roomNum { get; set; }
		public ILevelModel levelModel { get; private set; }
		public IPlayerModel playerModel { get; set; }

		public float playerPosX { get; set; }

		public bool levelInProgress{ get; set; }

		public bool isRoomLevel{ get { return levelId > ROOM_LEVEL_TYPE; } }

		public void switchLevel (int roomNum = 0)
		{
			if (roomNum > 0) {
				levelId = levelId * ROOM_LEVEL_TYPE + roomNum;
			} else {
				levelId = (int) levelId / ROOM_LEVEL_TYPE;
			}
		}

		public void initLevelData(IGameConfig gameConfig)
		{
			updateLevelModel (gameConfig);

			playerModel = new PlayerModel (playerId, gameConfig);
		}

		public void updateLevelModel(IGameConfig gameConfig)
		{
			levelModel = new LevelModel();
			ILevelConfigVO levelConfigVO = gameConfig.levelConfig.getConfigById (levelId);
			levelModel.setHasEnemy (levelConfigVO.hasEnemy);

			levelModel.bgAssetInfo = gameConfig.assetConfig.getBgAssetById (levelId).clone();
			if (isRoomLevel) {
				Vector2 minXAndY = levelModel.bgAssetInfo.minXAndY;
				levelModel.bgAssetInfo.minXAndY = new Vector2 (minXAndY.x + playerPosX, minXAndY.y);

				Vector2 maxXAndY = levelModel.bgAssetInfo.maxXAndY;
				levelModel.bgAssetInfo.maxXAndY = new Vector2 (maxXAndY.x + playerPosX, maxXAndY.y);
			}
		}

		#endregion
	}
}

