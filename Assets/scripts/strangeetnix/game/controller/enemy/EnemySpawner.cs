//Since we have an injectable RoutineRunner, we can move
//some behaviours that should be Controllers out of the classification of View.

//Here's an example. This mechanism simply waits a few seconds, then spawns a new enemy.
//Since it's pure controller, there's no good reason for it to be taking up space/
//cycles in the visual portion of the game. So we simply write it as a Controller
//and inject in the MonoBehaviour bit.

using System;
using strange.extensions.mediation.impl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace strangeetnix.game
{
	public class EnemySpawner : ISpawner
	{
		[Inject]
		public CreateEnemySignal createEnemySignal{ get; set; }

		[Inject]
		public IScreenUtil screenUtil{ get; set; }

		[Inject]
		public IRoutineRunner routineRunner{ get; set; }

		[Inject]
		public IGameModel gameModel{ get; set; }

		//Arguably these should be in a game config somewhere
		private float spawnSeconds;
		private List<int> _enemyIdList;

		private bool running = false;

		private IBgAssetVO _bgAssetInfo;


		//PostConstruct methods fire automatically after Construction
		//and after all injections are satisfied. It's a safe place
		//to do things you'd usually sonsider doing in the Constructor.
		[PostConstruct]
		public void PostConstruct()
		{
		}

		public void init ()
		{
			_bgAssetInfo = gameModel.levelModel.bgAssetInfo;

			spawnSeconds = gameModel.levelModel.waveVO.encounter_speed;
			if (spawnSeconds == 0) {
				spawnSeconds = 10;
			}

			_enemyIdList = new List<int> (gameModel.levelModel.enemyIdList);
			if (_enemyIdList == null || _enemyIdList.Count == 0) {
				Debug.LogError ("EnemySpawner. Wave Encounter Id List is null!");
			}
		}

		public void start ()
		{
			running = true;
			routineRunner.StartCoroutine (spawn ());
		}

		public void stop()
		{
			running = false;
		}

		public bool isNobodyToSpawn
		{
			get { return _enemyIdList.Count == 0; }
		}

		IEnumerator spawn()
		{
			while (running)
			{
				yield return new WaitForSeconds (spawnSeconds);
				if (running) {
					spawnEnemy ();
				}
			}
		}

		private void spawnEnemy()
		{
			if (_enemyIdList.Count > 0) {
				const int offset = 3;
				float startPosX = screenUtil.RandomPositionX (_bgAssetInfo.minXAndY.x + offset, _bgAssetInfo.maxXAndY.x - offset);
				createEnemySignal.Dispatch (_enemyIdList [0], startPosX);
				_enemyIdList.RemoveAt (0);
			} else {
				stop ();
			}
		}
	}
}

