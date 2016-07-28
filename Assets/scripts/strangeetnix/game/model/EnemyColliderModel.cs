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

		private CharacterStates _state;

		public BoxCollider2D collider { get; set; }
		public CharacterStates state { get { return _state; }}

		private EnemyTriggerManager _manager;

		private IEnumerator _waitToAction = null;
		private IEnumerator _waitBeforeEnemy = null;

		public EnemyColliderModel (EnemyView view1, EnemyTriggerManager manager)
		{
			view = view1;
			name = view.gameObject.name;
			collider = view.GetComponent<BoxCollider2D> ();

			_state = CharacterStates.CREATE;
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
				setState (CharacterStates.MOVE);
			}
		}

		public void setState(CharacterStates state, bool isForce=false)
		{
			Debug.Log ("EnemyColliderModel. id = " + name + ". state = " + state.ToString());
			if (_state != state || isForce) {
				stopWait ();
				_state = state;
				switch (state) {
				case CharacterStates.MOVE:
					//view.canHit = true;
					isPlayerTrigger = false;
					view.setState (state);
					break;

				case CharacterStates.HIT:
					isPlayerTrigger = true;
					if (view.isMove) {
						setState (CharacterStates.IDLE);
					}
					if (view.canHit) {
						view.setState (state, true);
						_waitToAction = waitToAction ();
						startWait (_waitToAction, TIME_TO_ATTACK);
					} else {
						setState (CharacterStates.WAIT_TO_HIT);
					}
					break;

				case CharacterStates.IDLE:
					if (view.isMove) {
						view.setState (state);
					}
					break;

				case CharacterStates.BEFORE_ENEMY:
					_waitBeforeEnemy = waitBeforeEnemy ();
					startWait (_waitBeforeEnemy, TIME_TO_MOVE);
					break;

				case CharacterStates.DEATH:
					stopCoroutines ();
					view.setState (state);
					break;

				case CharacterStates.DEFEAT:
					view.setState (state);
					_waitToAction = waitToAction ();
					startWait (_waitToAction, TIME_TO_DEFEAT);
					break;

				case CharacterStates.WAIT_TO_HIT:
					_waitToAction = waitToAction ();
					startWait (_waitToAction, TIME_BEFORE_HIT);
					break;
				}
			}
		}

		private void stopCoroutines()
		{
			if (_waitToAction != null) {
				routineRunner.StopCoroutine (_waitToAction);
			}

			if (_waitBeforeEnemy != null) {
				routineRunner.StopCoroutine (_waitBeforeEnemy);
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
				case CharacterStates.HIT:
				case CharacterStates.WAIT_TO_HIT:
					routine = _waitToAction;
					break;
				case CharacterStates.BEFORE_ENEMY:
					routine = _waitBeforeEnemy;
					break;
				}

				_isWait = false;
				_waitTime = 0;
				if (routine != null) {
					routineRunner.StopCoroutine (routine);
				}
			}
		}

		IEnumerator waitBeforeEnemy()
		{
			yield return new WaitForSeconds (_waitTime);
			Debug.Log ("EnemyColliderModel.waitBeforeEnemy id = " + name + ".isWait = " + _isWait.ToString() + ". state = " + _state.ToString ());
			if (_isWait) {				
				_isWait = false;
				doBeforeEnemy ();
			}
		}

		private void doBeforeEnemy()
		{
			if (triggerKeyBefore != null) {
				//bug!
				if (view.isMove) {
					setState (CharacterStates.IDLE);
				}

				setState (CharacterStates.BEFORE_ENEMY, true);
			} else {
				doAfterWait ();
			}
		}

		IEnumerator waitToAction()
		{
			yield return new WaitForSeconds (_waitTime);
			Debug.Log ("EnemyColliderModel.waitToAction, id = " + name + ".isWait = " + _isWait.ToString() + ". state = " + _state.ToString ());
			if (_isWait) {
				_isWait = false;
				doAfterWait ();
			}
		}

		private void doAfterWait()
		{
			if (isPlayerTrigger) {
				if (view.canHit && _state != CharacterStates.HIT) {
					setState (CharacterStates.HIT);
				} else {
					if (_state == CharacterStates.BEFORE_ENEMY) {
						Debug.Log ("fuck!");
					}
					if (_manager.isCollisionOut(collider)) {
						isPlayerTrigger = false;
						setState (CharacterStates.MOVE);
					} else {
						setState (CharacterStates.WAIT_TO_HIT);
					}
				}
			} else {
				setState (CharacterStates.MOVE);
			}
		}
	}
}

