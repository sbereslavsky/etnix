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

		public void addEnemy(EnemyView enemyView)
		{
			EnemyColliderModel colliderModel = new EnemyColliderModel (enemyView);
			colliderModel.state = EnemyStates.MOVE;
			_list.Add (colliderModel.name, colliderModel);
		}

		public void removeEnemy(EnemyView enemyView)
		{
			string enemyKey = enemyView.gameObject.name;
			if (_list.ContainsKey (enemyKey)) {
				_list.Remove(enemyKey);
			}
		}

		public void addTrigger(string mainKey, string colliderKey = null)
		{
			bool isPlayer = (colliderKey == null);
			if (_list.ContainsKey(mainKey)) {
				EnemyColliderModel mainColliderModel = _list [mainKey];
				EnemyView enemyView = mainColliderModel.view;

				if (enemyView.isMove) {
					enemyView.setState (EnemyStates.IDLE);
				}

				if (isPlayer) {
					mainColliderModel.isPlayerTrigger = true;

					if (enemyView.canHit) {
						enemyView.setState (EnemyStates.HIT);
					} else {
						enemyView.setState (EnemyStates.WAIT_TO_HIT);
					}
				} else {
					EnemyColliderModel otherColliderModel = _list [colliderKey];
					bool isMainBefore = isFirstBefore (mainColliderModel.view, otherColliderModel.view);
					if (isMainBefore) {
						mainColliderModel.triggerKeyAfter = colliderKey;
					} else {
						otherColliderModel.triggerKeyBefore = mainKey;
					}

					if (enemyView.currentState != EnemyStates.BEFORE_ENEMY) {
						enemyView.setState (EnemyStates.BEFORE_ENEMY);
					}
				}
			}
			else {
				Debug.LogError ("EnemyManager. Dictionary not contains key = " + mainKey);
			}
		}

		public void exitTrigger(string mainKey, string colliderKey = null)
		{
			
		}

		public void setEnemyState(EnemyView enemyView, EnemyStates state)
		{
			switch (state) {
			case EnemyStates.HIT:
				if (enemyView.canHit) {
					enemyView.setState (state);
				} else {
					enemyView.setState (EnemyStates.WAIT_TO_HIT);
				}
				break;
			case EnemyStates.IDLE:
				if (enemyView.isMove) {
					enemyView.setState (state);
				}
				break;
			case EnemyStates.BEFORE_ENEMY:
				enemyView.setState (state);
				break;
			}
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

