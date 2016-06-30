//The "View" for an Enemy. This MonoBehaviour is attached to the enemy prefab inside Unity.

using System;
using UnityEngine;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;
using System.Collections;
using System.Collections.Generic;

namespace strangeetnix.game
{
	public class EnemyView : CharacterView
	{
		public static string ID = "Enemy";

		private string EXPLOSION 	= "explosion";
		private string HEALTH		= "health";
		private string HEALTH_BG	= "healthBg";

		private string WALL_LEFT 	= "wallLeft";
		private string WALL_RIGHT 	= "wallRight";

		private float TIME_BEFORE_HIT		= 0.5f;
		private float TIME_TO_MOVE		= 1.1f;
		private float TIME_TO_ATTACK	= 1.5f;

		internal Signal<GameObject> triggerEnterSignal = new Signal<GameObject> ();
		internal Signal<GameObject> triggerExitSignal = new Signal<GameObject> ();

		internal Signal<GameObject> forceExitTriggerSignal = new Signal<GameObject> ();

		internal Signal<int> hitByPlayerSignal = new Signal<int> ();
		internal Signal enterCollisionSignal = new Signal ();

		public float moveSpeed = 2f;			// The speed the enemy moves at.
		public bool canHit { get; private set; }
		public bool isMove { get { return (_speed > 0); }}
		public EnemyStates currentState { get { return _state; } }

		private Vector3 _hpScale;
		private Transform _hpTransform;
		private Transform _hpBgTransform;

		private bool _canMove = true;
		private Collider2D _playerTrigger = null;
		private List<Collider2D> _enemyTriggerList = null;


		private float _speed = 0;

		private EnemyStates _state;

		public override void init()
		{
			// Setting up the references.
			_explosion = transform.Find(EXPLOSION).transform;
			_hpTransform = transform.Find (HEALTH).transform;
			_hpBgTransform = transform.Find (HEALTH_BG).transform;
			_hpScale = _hpTransform.localScale;
			canHit = true;

			_enemyTriggerList = new List<Collider2D> (3);

			base.init ();
			_state = EnemyStates.NULL;
			setState (EnemyStates.MOVE);

			checkToFlip ();
		}

		internal Vector2 explosionPos
		{
			get {
				return new Vector2 (_explosion.position.x, _explosion.position.y);
			}
		}

		private void checkToFlip()
		{
			GameObject playerGO = GameObject.FindGameObjectWithTag (PlayerView.ID);
			if (playerGO) {
				Transform player = playerGO.transform;
				if (player.position.x > transform.position.x) {
					flip ();
				}
			}
		}
			
		internal void setCanMove(bool value) 
		{
			_canMove = value;
			if (_canMove) {
				checkToFlip ();
				setState(EnemyStates.MOVE);
			}
		}

		internal void setCanHit(bool value) 
		{
			canHit = value;

			if (canHit && _playerTrigger && _speed == 0) {
				setState(EnemyStates.HIT);
			}
		}

		void OnTriggerEnter2D(Collider2D other)
		{
			//Debug.Log ("OnTriggerEnter2D. name = " + other.gameObject.name + ", state = " + _state.ToString());
			string otherName = other.gameObject.name;
			if (otherName == WALL_LEFT || otherName == WALL_RIGHT) {
				flip ();
			}
			else { 
				bool isPlayer = isPlayerObject (other.tag);
				if (isPlayer || (isOtherEnemy (other) && isEqualsScaleX (other.gameObject))) {
					GameObject otherGO = (isPlayer) ? null : other.gameObject;
					triggerEnterSignal.Dispatch (otherGO);
				}
			}

			//Debug.Log("Something has entered this zone.");    
		}  

		void OnTriggerStay2D(Collider2D other)
		{
			//Debug.Log ("OnTriggerStay2D. name = " + other.gameObject.name + ", state = " + _state.ToString());
			if (_speed == 0 && _isWait && isCollisionOut(other) && isPlayerObject (other.tag)) {
				_playerTrigger = null;
				forceExitTriggerSignal.Dispatch (other.gameObject);
			}
			/*else if (isEqualsNames(other.gameObject) && _state == EnemyStates.BEFORE_ENEMY) {
				//playMove ();
			}*/
		}

		void OnTriggerExit2D(Collider2D other)
		{
			//Debug.Log ("OnTriggerExit2D. name = " + other.gameObject.name + ", _speed = " + _speed + ", state = " + _state.ToString ());
			bool isPlayer = isPlayerObject (other.tag);
			if (isPlayer || isOtherEnemy (other)) {
				GameObject otherGO = (isPlayer) ? null : other.gameObject;
				triggerExitSignal.Dispatch (otherGO);
			}

			if (_enemyTriggerList.Contains (other)) {
				_enemyTriggerList.Remove (other);
			}

			if (_playerTrigger == other) {
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
			}
		}

		internal void onExitTrigger(Collider2D other)
		{
			OnTriggerExit2D (other);
		}

		IEnumerator waitToMove()
		{
			yield return new WaitForSeconds (_waitTime);
			_isWait = false;
			if (_enemyTriggerList.Count > 0) {
				startWait (waitToMove(), TIME_TO_MOVE);
			} else {
				doAfterHit ();
			}
		}

		IEnumerator waitToHit()
		{
			yield return new WaitForSeconds (_waitTime);
			_isWait = false;
			doAfterHit ();
		}

		private void doAfterHit()
		{
			if (_playerTrigger) {
				if (canHit) {
					setState (EnemyStates.HIT);
				} else {
					setState (EnemyStates.WAIT_TO_HIT);
				}
			} else {
				setState (EnemyStates.MOVE);
			}
		}

		IEnumerator onAttackComplete () 
		{
			yield return new WaitForSeconds (_waitTime);
			_isWait = false;
			if (_playerTrigger) {
				setState (EnemyStates.WAIT_TO_HIT);
			} else {
				setState (EnemyStates.MOVE);
			}
		}

		internal void setState(EnemyStates value, bool isForce=false)
		{
			if (_state != value || isForce) {
				switch (value) {
				case EnemyStates.MOVE:
					_playerTrigger = null;
					playMove ();
					break;

				case EnemyStates.HIT:
					setState (EnemyStates.IDLE, true);
					postHitPlayer ();
					break;

				case EnemyStates.DEFEAT:
					break;

				case EnemyStates.DEATH:
				case EnemyStates.IDLE:
					if (_speed > 0) {
						stopMove ();
					}
					break;

				case EnemyStates.WAIT_TO_HIT:
					startWait (waitToHit(), TIME_BEFORE_HIT);
					break;

				case EnemyStates.BEFORE_ENEMY:
					startWait (waitToMove(), TIME_TO_MOVE);
					break;
				}

				_state = value;
			}
		}

		void FixedUpdate ()
		{
			if (_canMove && !isPlayAnimation (EnemyAnimatorTypes.TRIGGER_DEFEAT)) {
				if (_isWait || _playerTrigger || _state == EnemyStates.BEFORE_ENEMY) {
					return;
				}

				playMove ();

				// Set the enemy's velocity to moveSpeed in the x direction.
				if (_speed > 0) {
					_rigidBody.velocity = new Vector2(-transform.localScale.x * moveSpeed, _rigidBody.velocity.y);	
				}
			}
		}

		private void postHitPlayer()
		{
			playAnimation (EnemyAnimatorTypes.TRIGGER_HIT);
			startWait (onAttackComplete(), TIME_TO_ATTACK);

			enterCollisionSignal.Dispatch();
		}

		internal void hitByPlayer()
		{
			// Reduce the number of hit points by one.
			if (_anim && !isPlayAnimation(EnemyAnimatorTypes.TRIGGER_DEFEAT) && !isPlayAnimation(EnemyAnimatorTypes.TRIGGER_HIT)) {
				playAnimation (EnemyAnimatorTypes.TRIGGER_DEFEAT);
				stopMove ();
			}
		}

		internal void stopMove()
		{
			if (_speed > 0) {
				setSpeed (false);
			}
		}

		private void playMove()
		{
			if (_speed == 0) {
				forceExitTriggerSignal.Dispatch (null);
				setSpeed (true);
			}
		}

		private void setSpeed(bool isMove=false)
		{
			_speed = (isMove) ? 0.1f : 0f;
			_anim.SetFloat(PlayerAnimatorTypes.FLOAT_SPEED, _speed);

			if (_speed == 0) {				
				_rigidBody.velocity = Vector2.zero;
			}
		}

		override public void setDeath()
		{
			base.setDeath ();
			setState (EnemyStates.DEATH);
			playAnimation (EnemyAnimatorTypes.TRIGGER_DEATH);

			if (_playerTrigger) {
				PlayerView playerView = _playerTrigger.gameObject.GetComponent<PlayerView> ();
				if (playerView) {
					playerView.onExitTrigger (_collider2d);
				}
			}

			if (_enemyTriggerList.Count > 0) {
				foreach (Collider2D c in _enemyTriggerList) {
					if (c != null) {
						exitFromEnemyTrigger (c);
					}
				}
				_enemyTriggerList.Clear ();
			}
		}

		private void exitFromEnemyTrigger(Collider2D enemyCollider)
		{
			EnemyView enemyView = enemyCollider.gameObject.GetComponent<EnemyView> ();
			if (enemyView) {
				enemyView.onExitTrigger (_collider2d);
			}
		}

		private PlayerView getPlayerView(Collider2D collider2d)
		{
			PlayerView playerView = collider2d.gameObject.GetComponent<PlayerView> ();
			return playerView;
		}

		internal void updateHp(int currentHp, int startHp)
		{
			if (startHp > 0) {
				int hpKoef = 100 * currentHp / startHp;
				_hpTransform.localScale = new Vector3(_hpScale.x * hpKoef * 0.01f, _hpScale.y, 1);
				if (hpKoef == 0) {
					_hpBgTransform.localScale = new Vector3(0, _hpScale.y, 1);
				}
			} else {
				Debug.LogError ("updateEnemyHp.startHP=0!");
			}
		}
	}
}
