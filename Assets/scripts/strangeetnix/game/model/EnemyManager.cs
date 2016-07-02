using System;
using System.Collections.Generic;
using UnityEngine;

namespace strangeetnix.game
{
	public class EnemyManager : IEnemyManager
	{
		private Dictionary<string, EnemyColliderModel> _list;

		//private bool _canBeat = false;

		public EnemyManager ()
		{	
			if (_list == null) {
				_list = new Dictionary<string, EnemyColliderModel> ();
			} 
		}

		public void reset ()
		{
			if (_list != null && _list.Count > 0) {
				_list.Clear ();
			}
		}

		public void addEnemyView (EnemyView enemyView, IRoutineRunner routineRunner)
		{
			EnemyColliderModel colliderModel = new EnemyColliderModel (enemyView);
			if (colliderModel != null && _list.ContainsKey (colliderModel.name)) {
				colliderModel.setState (EnemyStates.MOVE);
				colliderModel.routineRunner = routineRunner;
				_list.Add (colliderModel.name, colliderModel);
			} else {
				Debug.LogWarning ("EnemyManager.addEnemyView. An element with the same key already exists in the dictionary!");
			}
		}

		public void removeEnemy(string enemyKey)
		{
			EnemyColliderModel mainColliderModel = getColliderModel (enemyKey);
			if (mainColliderModel != null) {
				removeTriggerKey (mainColliderModel, enemyKey);
			}

			if (_list.ContainsKey (enemyKey)) {
				_list.Remove(enemyKey);
			}
		}

		public void addTrigger(string mainKey, string colliderKey = null)
		{
			bool isPlayer = (colliderKey == null);
			EnemyColliderModel mainColliderModel = getColliderModel (mainKey);
			if (mainColliderModel != null) {
				EnemyView enemyView = mainColliderModel.view;
				if (enemyView) {
					mainColliderModel.setState (EnemyStates.IDLE);

					if (isPlayer) {
						mainColliderModel.setState (EnemyStates.HIT);
					} else {
						EnemyColliderModel otherColliderModel = getColliderModel (colliderKey);
						if (otherColliderModel != null) {
							bool isMainBefore = isFirstBefore (mainColliderModel.view, otherColliderModel.view);
							if (isMainBefore) {
								mainColliderModel.triggerKeyAfter = colliderKey;
							} else {
								otherColliderModel.triggerKeyBefore = mainKey;
							}

							mainColliderModel.setState (EnemyStates.BEFORE_ENEMY);
						}
					}
				}
			}
		}

		public void exitTrigger(string mainKey, GameObject enemyGO)
		{
			bool isPlayer = false; 
			string colliderKey = null;
			if (enemyGO != null) {
				if (enemyGO.tag == PlayerView.ID) {
					isPlayer = true;
				} else {
					colliderKey = enemyGO.name;
				}
			}

			EnemyColliderModel mainColliderModel = getColliderModel (mainKey);
			if (mainColliderModel != null && mainColliderModel.view != null) {
				if (isPlayer) {
					mainColliderModel.isPlayerTrigger = false;
					mainColliderModel.setState (EnemyStates.MOVE);
				} else {
					mainColliderModel.isPlayerTrigger = false;

					removeTriggerKey (mainColliderModel, mainKey);

					/*EnemyColliderModel colliderModel = getColliderModel (colliderKey);
					if (colliderModel != null) {
						removeTriggerKey (colliderModel, mainKey);
					}*/
				}
				/*if (_playerTrigger == other) {
					if (_isWait) {
						_isWait = false;
						StopAllCoroutines ();
					}
					_playerTrigger = null;
					setState(EnemyStates.MOVE);
				}

				if (isOtherEnemy (other)) {
					if (_isWait) {
						_isWait = false;
						StopAllCoroutines ();
					}

					if (_state != EnemyStates.MOVE) {
						setState (EnemyStates.MOVE);
					}
				}*/
			}
		}

		public void setState(string mainKey, EnemyStates state)
		{
			EnemyColliderModel mainColliderModel = getColliderModel (mainKey);
			if (mainColliderModel != null) {
				mainColliderModel.setState (state);
			}
		}

		private void removeTriggerKey(EnemyColliderModel mainColliderModel, string key)
		{
			if (mainColliderModel != null) {
				EnemyColliderModel colliderModel;
				if (mainColliderModel.triggerKeyAfter != null) {
					colliderModel = getColliderModel (mainColliderModel.triggerKeyAfter);
					if (colliderModel != null) {
						colliderModel.removeKey (key);
						colliderModel.setState (EnemyStates.MOVE);
					}
				}

				//that never not run!
				if (mainColliderModel.triggerKeyBefore != null) {
					colliderModel = getColliderModel (mainColliderModel.triggerKeyBefore);
					if (colliderModel != null) {
						colliderModel.removeKey (key);
					}
				}
			}
		}

		internal EnemyColliderModel getColliderModel(string key)
		{
			if (_list.ContainsKey (key)) {
				return _list [key];
			} else {
				Debug.LogWarning ("EnemyManager. Dictionary not contains key = " + key);
			}

			return null;
		}

		private bool isFirstBefore(EnemyView enemyView1, EnemyView enemyView2)
		{
			bool scaleTurn = enemyView1.gameObject.transform.localScale.x > 0;
			GameObject beforeEnemyGO = enemyView2.gameObject;
			if (scaleTurn && enemyView1.gameObject.transform.position.x < enemyView2.gameObject.transform.position.x ||
				!scaleTurn && enemyView1.gameObject.transform.position.x > enemyView2.gameObject.transform.position.x ) {
				return true;
			}

			return false;
		}
	}
}

