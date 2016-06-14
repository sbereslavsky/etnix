//The API for a model representing how the player is doing

using System;

namespace strangeetnix.game
{
	public interface IGameModel
	{
		int playerId { get; set; }
		int createEnemyId { get; set; }
		int levelId { get; set; }

		int roomNum { get; set; }

		ILevelModel levelModel { get; }
		IPlayerModel playerModel { get; set; }

		float playerPosX { get; set; }
		void resetPlayerPosX ();

		bool isRoomLevel{ get; }
		bool levelInProgress{ get; set; }

		void Reset();
		void initLevelData (IGameConfig gameConfig);
		void switchLevel (int roomNum);
		void updateLevelModel (IGameConfig gameConfig);
	}
}

