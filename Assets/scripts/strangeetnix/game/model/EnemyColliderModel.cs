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

		public EnemyColliderModel (EnemyView view1)
		{
			view = view1;
			name = view.gameObject.name;

			_state = EnemyStates.NULL;

			isPlayerTrigger = false;
			triggerKeyBefore = null;
			triggerKeyAfter = null;
		}

		public void removeKey(string key)
		{
			if (triggerKeyAfter == key) {
				triggerKeyAfter = null;
			}
			else if (triggerKeyBefore == key) {
				triggerKeyBefore = null;
				setState (EnemyStates.MOVE);
			}
		}

		public Collider2D getCollider2D()
		{
			return view.GetComponent<Collider2D> ();
		}

		public void setState(EnemyStates state)
		{
			stopWait ();
			_state = state;
			switch (state) {
			case EnemyStates.MOVE:
				view.setState (state);
				break;

			case EnemyStates.HIT:
				isPlayerTrigger = true;
				if (view.canHit) {
					setState (EnemyStates.IDLE);
					view.setState (state);
					startWait (onHitComplete(), TIME_TO_ATTACK);
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

			case EnemyStates.DEFEAT:
				startWait (waitToDefeat(), TIME_TO_MOVE);
				break;

			case EnemyStates.WAIT_TO_HIT:
				startWait (waitToHit(), TIME_BEFORE_HIT);
				break;
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
				case EnemyStates.WAIT_TO_HIT:
					routine = waitToHit ();
					break;
				case EnemyStates.BEFORE_ENEMY:
					routine = waitBeforeEnemy ();
					break;
				case EnemyStates.DEFEAT:
					routine = waitToDefeat ();
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
			_isWait = false;
			doBeforeEnemy ();
		}

		IEnumerator waitToHit()
		{
			yield return new WaitForSeconds (_waitTime);
			_isWait = false;
			doAfterHit ();
		}

		IEnumerator waitToDefeat()
		{
			yield return new WaitForSeconds (_waitTime);
			_isWait = false;
			doBeforeEnemy ();
		}

		private void doBeforeEnemy()
		{
			if (triggerKeyBefore != null) {
				setState (EnemyStates.BEFORE_ENEMY);
			} else {
				doAfterHit ();
			}
		}

		private void doAfterHit()
		{
			if (isPlayerTrigger) {
				if (view.canHit) {
					setState (EnemyStates.HIT);
				} else {
					setState (EnemyStates.WAIT_TO_HIT);
				}
			} else {
				setState (EnemyStates.MOVE);
			}
		}

		IEnumerator onHitComplete () 
		{
			yield return new WaitForSeconds (_waitTime);
			_isWait = false;
			if (isPlayerTrigger) {
				setState (EnemyStates.WAIT_TO_HIT);
			} else {
				setState (EnemyStates.MOVE);
			}
		}
	}
}

