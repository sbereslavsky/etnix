using System;
using System.Collections;
using UnityEngine;

namespace strangeetnix.game
{
	public class EnemyColliderModel
	{
		public IRoutineRunner routineRunner { get; set; }

		public EnemyView view { get; private set; }
		public String name { get; private set; }

		public string triggerKeyBefore { get; set; }  
		public string triggerKeyAfter { get; set; }

		public bool isPlayerTrigger { get; set; }

		private float TIME_BEFORE_HIT	= 0.5f;
		private float TIME_TO_MOVE		= 1.1f;
		private float TIME_TO_ATTACK	= 1.5f;
		private float TIME_TO_DEFEAT	= 1.2f;

		protected bool _isWait = false;
		protected float _waitTime = 0f;

		private EnemyStates _state;

		public BoxCollider2D collider { get; set; }
		public EnemyStates state { get { return _state; }}

		private EnemyTriggerManager _manager;

		public EnemyColliderModel (EnemyView view1, EnemyTriggerManager manager)
		{
			view = view1;
			name = view.gameObject.name;
			collider = view.GetComponent<BoxCollider2D> ();

			_state = EnemyStates.NULL;
			_manager = manager;

			isPlayerTrigger = false;
			triggerKeyBefore = null;
			triggerKeyAfter = null;
		}

		public void removeKey(string key, bool isBefore=false)
		{
			if (triggerKeyAfter == key) {
				triggerKeyAfter = null;
			}
			else if (triggerKeyBefore == key) {
				triggerKeyBefore = null;
				setState (EnemyStates.MOVE);
			}
		}

		public void setState(EnemyStates state, bool isForce=false)
		{
			Debug.Log ("EnemyColliderModel. id = " + name + ". state = " + state.ToString());
			if (_state != state || isForce) {
				stopWait ();
				_state = state;
				switch (state) {
				case EnemyStates.MOVE:
					//view.canHit = true;
					isPlayerTrigger = false;
					view.setState (state);
					break;

				case EnemyStates.HIT:
					isPlayerTrigger = true;
					if (view.isMove) {
						setState (EnemyStates.IDLE);
					}
					if (view.canHit) {
						view.setState (state, true);
						startWait (waitToAction(), TIME_TO_ATTACK);
					} else {
						setState (EnemyStates.WAIT_TO_HIT);
					}
					break;

				case EnemyStates.IDLE:
					if (view.isMove) {
						view.setState (state);
					}
					break;

				case EnemyStates.BEFORE_ENEMY:
					startWait (waitBeforeEnemy(), TIME_TO_MOVE);
					break;

				case EnemyStates.DEATH:
					view.setState (state);
					break;

				case EnemyStates.DEFEAT:
					view.setState (state);
					startWait (waitToAction(), TIME_TO_DEFEAT);
					break;

				case EnemyStates.WAIT_TO_HIT:
					startWait (waitToAction(), TIME_BEFORE_HIT);
					break;
				}
			}
		}

		protected void startWait(IEnumerator routine, float time)
		{
			if (!_isWait) {
				_waitTime = time;
				_isWait = true;
				routineRunner.StartCoroutine (routine);
			}
		}

		private void stopWait()
		{
			if (_isWait) {
				IEnumerator routine = null;
				switch (_state) {
				case EnemyStates.HIT:
				case EnemyStates.WAIT_TO_HIT:
					routine = waitToAction ();
					break;
				case EnemyStates.BEFORE_ENEMY:
					routine = waitBeforeEnemy ();
					break;
				}

				if (routine != null) {
					routineRunner.StopCoroutine (routine);
				}
			}
		}

		IEnumerator waitBeforeEnemy()
		{
			yield return new WaitForSeconds (_waitTime);
			Debug.Log ("EnemyColliderModel.waitBeforeEnemy id = " + name);
			_isWait = false;
			doBeforeEnemy ();
		}

		private void doBeforeEnemy()
		{
			if (triggerKeyBefore != null) {
				//bug!
				if (view.isMove) {
					setState (EnemyStates.IDLE);
				}

				setState (EnemyStates.BEFORE_ENEMY, true);
			} else {
				doAfterWait ();
			}
		}

		IEnumerator waitToAction()
		{
			yield return new WaitForSeconds (_waitTime);
			Debug.Log ("EnemyColliderModel.waitToAction, id = " + name + ". state = " + _state.ToString());
			_isWait = false;
			doAfterWait ();
		}

		private void doAfterWait()
		{
			if (isPlayerTrigger) {
				if (view.canHit && _state != EnemyStates.HIT) {
					setState (EnemyStates.HIT);
				} else {
					if (_state == EnemyStates.BEFORE_ENEMY) {
						Debug.Log ("fuck!");
					}
					if (_manager.isCollisionOut(collider)) {
						isPlayerTrigger = false;
						setState (EnemyStates.MOVE);
					} else {
						setState (EnemyStates.WAIT_TO_HIT);
					}
				}
			} else {
				setState (EnemyStates.MOVE);
			}
		}
	}
}

