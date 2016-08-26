using System;
using System.Collections.Generic;
using strange.extensions.command.impl;
using UnityEngine;
using strange.extensions.pool.api;

namespace strangeetnix.game
{
	public class CreateCoinsCommand : Command
	{
		//The named GameObject that parents the rest of the game area
		[Inject(GameElement.GAME_FIELD)]
		public GameObject gameField{ get; set; }

		[Inject]
		public IResourceManager resourceManager { get; set; }

		[Inject]
		public IGameModel gameModel { get; set; }

		//The position to place the coin.
		[Inject]
		public Vector2 position{ get; set; }

		//The coin value.
		[Inject]
		public int coinValue{ get; set; }

		public override void Execute ()
		{
			//create coin gameobject
			//GameObject coinStyle = Resources.Load<GameObject> (AssetConfig.COIN.path);
			GameObject coinStyle = resourceManager.getResourceByAssetData (AssetConfig.COIN);
			GameObject coinGO = GameObject.Instantiate (coinStyle) as GameObject;
			coinGO.transform.localPosition = new Vector3(position.x, position.y + 0.1f, 0);
			coinGO.transform.parent = gameField.transform;

			if (coinGO.GetComponent<DropCoinView> () == null) {
				coinGO.AddComponent<DropCoinView> ();
			}

			DropCoinView dropCoinView = coinGO.GetComponent<DropCoinView> ();
			if (dropCoinView != null) {
				dropCoinView.coinValue = coinValue;
			}
		}
	}
}

