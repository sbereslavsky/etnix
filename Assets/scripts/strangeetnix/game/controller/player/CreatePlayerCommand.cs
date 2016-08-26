//Create the ShipView

//NOTE: We're not using pools to create the ShipView. Arguably we should...
//or at least store the single instance. As a practical matter, Ship creation
//and destruction is "rare" in game terms, happening only when the player gets
//killed or on the turnover of a level. The wastage of resources is therefore trivial.

using System;
using strange.extensions.command.impl;
using UnityEngine;

namespace strangeetnix.game
{
	public class CreatePlayerCommand : Command
	{
		[Inject(GameElement.GAME_FIELD)]
		public GameObject gameField{ get; set; }

		[Inject]
		public IGameModel gameModel { get; set; }

		[Inject]
		public IResourceManager resourceManager { get; set; }

		[Inject]
		public UpdateHudItemSignal updateHudItemSignal { get; set; }

		[Inject]
		public CameraEnabledSignal cameraEnabledSignal { get; set; }

		[Inject]
		public float position { get; set; }

		public override void Execute ()
		{
			if (injectionBinder.GetBinding<PlayerView> (GameElement.PLAYER) != null)
				injectionBinder.Unbind<PlayerView> (GameElement.PLAYER);

			IAssetVO playerAssetVO = gameModel.playerModel.assetVO;

			//add the player's ship
			GameObject playerStyle = resourceManager.getResourceByAssetData(playerAssetVO.assetData);// Resources.Load<GameObject> (playerAssetVO.assetData.path);
			GameObject playerGO = GameObject.Instantiate (playerStyle) as GameObject;
			playerGO.name = playerAssetVO.assetData.id;
			playerGO.tag = PlayerView.ID;
			playerGO.transform.localPosition = new Vector3(position, gameModel.roomModel.bgAssetInfo.startPosY, -0.05f);
			//playerGO.layer = LayerMask.NameToLayer("player");
			playerGO.transform.SetParent(gameField.transform, false);
			playerGO.AddComponent<PlayerView> ();
			PlayerView playerView = playerGO.GetComponent<PlayerView> ();
			playerView.moveForce = gameModel.playerModel.moveForce;
			playerView.maxSpeed = gameModel.playerModel.moveSpeed;

			injectionBinder.Bind<PlayerView> ().ToValue (playerView).ToName (GameElement.PLAYER);

			updateHudItemSignal.Dispatch (UpdateHudItemType.HP, gameModel.playerModel.hp);
			updateHudItemSignal.Dispatch (UpdateHudItemType.EXP, gameModel.playerModel.exp);

			//Whenever a ship is created, the game is on!
			gameModel.levelInProgress = true;
			cameraEnabledSignal.Dispatch (true);
		}
	}
}

