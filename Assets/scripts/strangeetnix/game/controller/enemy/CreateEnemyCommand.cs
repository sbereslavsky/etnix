//Newly created Enemies get pulled from their pool and placed at the given position.
//(See DestroyEnemyCommand for the bit where they're returned to their pool)

using System;
using System.Collections.Generic;
using strange.extensions.command.impl;
using UnityEngine;
using strange.extensions.pool.api;

namespace strangeetnix.game
{
	public class CreateEnemyCommand : Command
	{
		//The named GameObject that parents the rest of the game area
		[Inject(GameElement.GAME_FIELD)]
		public GameObject gameField{ get; set; }

		//The pool from which we draw Enemies (see also the GameContext's use of ResourceInstanceProvider).
		[Inject]
		public IEnemyPoolManager enemyPoolManager{ get; set; }

		[Inject]
		public IGameModel gameModel{ get; set; }

		//Higher level Enemies are smaller, faster, fire more often, and are worth more points.
		[Inject]
		public int id{ get; set; }

		//The position to place the blighter.
		[Inject]
		public float position{ get; set; }

		[Inject]
		public IRoutineRunner routineRunner { get; set; }

		public override void Execute ()
		{
			if (injectionBinder.GetBinding<PlayerView> (GameElement.PLAYER) != null) {
				gameModel.levelModel.enemyId++;
				gameModel.levelModel.enemyCount++;
				gameModel.createEnemyId = id;

				IEnemyModel enemyModel = gameModel.levelModel.getEnemyModelById (id);
				IAssetVO enemyAssetVO = enemyModel.assetVO;

				GameObject enemyGO = null;
				IPool<GameObject> pool = enemyPoolManager.getPoolByKey (enemyAssetVO.assetData.id);
				if (pool != null) {
					enemyGO = pool.GetInstance ();
					//GameObject enemyStyle = Resources.Load<GameObject> (enemyAssetVO.path);
					//enemyStyle.transform.localPosition = new Vector3(position, gameModel.levelModel.bgAssetInfo.startPosY, 0f);
					//GameObject enemyGO = GameObject.Instantiate (enemyStyle) as GameObject;

					enemyGO.transform.localPosition = new Vector3 (position, gameModel.levelModel.bgAssetInfo.startPosY, 0f);
					enemyGO.name = enemyAssetVO.assetData.id + gameModel.levelModel.enemyId;
					enemyGO.tag = EnemyView.ID;
					enemyGO.SetActive (true);
					if (enemyGO.transform.parent == null) {
						enemyGO.transform.SetParent (gameField.transform, false);
					}

					if (enemyGO.GetComponent <EnemyView> () == null) {
						enemyGO.AddComponent<EnemyView> ();
					}
					EnemyView enemyView = enemyGO.GetComponent<EnemyView> ();
					if (enemyView != null) {
						enemyView.poolKey = enemyAssetVO.assetData.id;
						gameModel.levelModel.enemyManager.addEnemyView (enemyView, routineRunner);
					}
				} else {
					Debug.LogError ("CreateEnemyCommand. pool is null!");
				}
			}
		}
	}
}

