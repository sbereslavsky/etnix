using System;
using System.Collections.Generic;
using UnityEngine;

namespace strangeetnix.game
{
	public class EnemyTriggerManager : IEnemyManager
	{
		[Inject]
		public IScreenUtil screenUtil { get; set; }

		public PlayerView playerView { get; private set; }
		private BoxCollider2D _playerCollider;

		private Dictionary<string, EnemyColliderModel> _list;

		//private bool _canBeat = false;

		public EnemyTriggerManager ()
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

		public void setPlayerView(PlayerView view)
		{
			playerView = view;
			_playerCollider = view.GetComponent<BoxCollider2D> ();
		}

		public List<GameObject> getEnemyToHit()
		{
			List<GameObject> list = new List<GameObject> (10);
			List<EnemyColliderModel> values = new List<EnemyColliderModel> (_list.Values);
			if (values.Count > 0) {
				foreach(EnemyColliderModel model in values) {
					if (model != null && model.isPlayerTrigger && !isCollisionOut(model.collider) && isEqualsScaleX(model.view.gameObject)) {
						list.Add (model.view.gameObject);
					}
				}
			}

			return list;
		}

		public void addEnemyView (EnemyView enemyView, IRoutineRunner routineRunner)
		{
			EnemyColliderModel colliderModel = new EnemyColliderModel (enemyView, this);
			if (colliderModel != null && !_list.ContainsKey (colliderModel.name)) {
				colliderModel.routineRunner = routineRunner;
				colliderModel.setState (EnemyStates.MOVE);
				_list.Add (colliderModel.name, colliderModel);
			} else {
				Debug.LogWarning ("EnemyManager.addEnemyView. An element with the same key = " + colliderModel.name + " already exists in the dictionary!");
			}
		}

		public void removeEnemy(string enemyKey, bool isForce = false)
		{
			EnemyColliderModel mainColliderModel = getColliderModel (enemyKey);
			if (mainColliderModel != null && !isForce) {
				removeKey (mainColliderModel.triggerKeyAfter, enemyKey);
				removeKey (mainColliderModel.triggerKeyBefore, enemyKey);
			}

			if (_list.ContainsKey (enemyKey)) {
				_list.Remove(enemyKey);
			}
		}

		public void addTrigger(string mainKey, string colliderKey = null)
		{
			bool isPlayer = (colliderKey == null);
			Debug.Log ("addTriger. isPlayer = "+isPlayer.ToString()+". mainkey = " + mainKey + ". otherKey = " + colliderKey);
			EnemyColliderModel mainColliderModel = getColliderModel (mainKey);
			if (mainColliderModel != null && mainColliderModel.view) {
				if (isPlayer) {
					stopCharacter (mainColliderModel);
					mainColliderModel.isPlayerTrigger = true;
					mainColliderModel.setState (EnemyStates.HIT);
				} else {
					EnemyColliderModel otherColliderModel = getColliderModel (colliderKey);
					if (otherColliderModel != null) {
						stopCharacter (mainColliderModel);
						bool isMainBefore = isFirstBefore (mainColliderModel.view, otherColliderModel.view);
						if (isMainBefore) {
							mainColliderModel.triggerKeyAfter = colliderKey;
						} else {
							mainColliderModel.triggerKeyBefore = colliderKey;
						}

						if (!mainColliderModel.isPlayerTrigger) {
							mainColliderModel.setState (EnemyStates.BEFORE_ENEMY);
						}
					}
				}
			}
		}

		private void stopCharacter(EnemyColliderModel mainColliderModel)
		{
			if (mainColliderModel != null && mainColliderModel.view.isMove) {
				mainColliderModel.setState (EnemyStates.IDLE);
			}
		}

		public bool checkBeforeHit(string mainKey)
		{
			bool result = false;
			bool isMove = false;
			EnemyColliderModel mainColliderModel = getColliderModel (mainKey);
			if (mainColliderModel != null) {
				isMove = mainColliderModel.view.isMove;
				if (!isCollisionOut (mainColliderModel.collider) && isPlayerBefore(mainColliderModel.view.gameObject)) {
					result = true;
				}
			}

			Debug.Log ("EnemyTriggerManager.name = " + mainKey + ". checkEnemyToHit = " + result + ". isMove = " + isMove.ToString());
			if (!result) {
				forceExitPlayerTrigger (mainColliderModel);
			}

			return result;
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

			Debug.Log ("exitTrigger. isPlayer = "+isPlayer.ToString()+". mainkey = " + mainKey + ". otherKey = " + colliderKey);
			EnemyColliderModel mainColliderModel = getColliderModel (mainKey);
			if (mainColliderModel != null && mainColliderModel.view != null) {				
				if (isPlayer) {
					mainColliderModel.isPlayerTrigger = false;
					if (!mainColliderModel.view.isMove) {
						mainColliderModel.setState (EnemyStates.MOVE);
					}
				} else if (colliderKey != null) {
					EnemyColliderModel colliderModel = getColliderModel (colliderKey);
					if (colliderModel != null && colliderModel.view != null) {
						bool isMainBefore = isFirstBefore (mainColliderModel.view, colliderModel.view);
						mainColliderModel.removeKey (colliderKey, isMainBefore);
						colliderModel.removeKey (mainKey, !isMainBefore);
					}
				}
			}
		}

		public void forceExitPlayerTrigger(string mainKey)
		{
			EnemyColliderModel mainColliderModel = getColliderModel (mainKey);
			if (mainColliderModel != null) {
				mainColliderModel.isPlayerTrigger = false;
				mainColliderModel.setState (EnemyStates.MOVE);
			}
		}

		public void forceExitPlayerTrigger(EnemyColliderModel mainColliderModel)
		{
			 if (mainColliderModel != null) {
				mainColliderModel.isPlayerTrigger = false;
				mainColliderModel.setState (EnemyStates.MOVE);
			}
		}

		public void setState(string mainKey, EnemyStates state)
		{
			EnemyColliderModel mainColliderModel = getColliderModel (mainKey);
			if (mainColliderModel != null) {
				mainColliderModel.setState (state);
			}
		}

		public EnemyStates getState(string mainKey)
		{
			EnemyColliderModel mainColliderModel = getColliderModel (mainKey);
			if (mainColliderModel != null) {
				return mainColliderModel.state;
			}

			return EnemyStates.NULL;
		}

		private void removeKey(string colliderKey, string deleteKey)
		{
			if (colliderKey != null && deleteKey != null) {
				EnemyColliderModel colliderModel = getColliderModel (colliderKey);
				if (colliderModel != null) {
					colliderModel.removeKey (deleteKey);
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

		protected bool isEqualsScaleX(GameObject go)
		{
			return playerView.gameObject.transform.localScale.x == go.transform.localScale.x;
		}

		private bool isPlayerBefore (GameObject enemy)
		{
			bool isTurnLeft = (enemy.transform.localScale.x > 0); 
			if (isTurnLeft) {
				return (playerView.gameObject.transform.position.x < enemy.gameObject.transform.position.x);
			} 

			return (playerView.gameObject.transform.position.x > enemy.gameObject.transform.position.x);
		}

		public bool isCollisionOut(BoxCollider2D other)
		{
			bool result = false;
			if (_playerCollider != null && other) {
				float dist1 = Math.Abs(other.bounds.center.x - _playerCollider.bounds.center.x);
				float width = other.size.x + _playerCollider.size.x;
				result = (width * 0.3f > dist1 || dist1 > width + 0.1f);
			}

			return result;
		}

		//first item - left, second - right
		public List<int> getEnemyBothSideCount ()
		{
			List<int> result = new List<int> {0, 0};
			float playerX = playerView.gameObject.transform.position.x;
			List<GameObject> list = new List<GameObject> (10);
			List<EnemyColliderModel> values = new List<EnemyColliderModel> (_list.Values);
			if (values.Count > 0) {
				foreach (EnemyColliderModel model in values) {
					if (model != null && model.state != EnemyStates.DEATH) {
						float enemyX = model.view.gameObject.transform.position.x;
						if (enemyX < playerX) {
							result[0]++;
						} else {
							result[1]++;
						}
					}
				}
			}

			return result;
		}
	}
}

