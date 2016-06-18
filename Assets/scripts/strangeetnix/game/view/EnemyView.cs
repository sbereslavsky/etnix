//The "View" for an Enemy. This MonoBehaviour is attached to the enemy prefab inside Unity.

using System;
using UnityEngine;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;
using System.Collections;

namespace strangeetnix.game
{
	public class EnemyView : CharacterView
	{
		public static string ID = "Enemy";

		private string FRONT_CHECK 	= "frontCheck";
		private string EXPLOSION 	= "explosion";
		private string HEALTH		= "health";
		private string HEALTH_BG	= "healthBg";

		private string WALL_LEFT 	= "wallLeft";
		private string WALL_RIGHT 	= "wallRight";

		private float TIME_TO_HIT		= 1.1f;
		private float TIME_TO_MOVE		= 1.1f;
		private float TIME_TO_ATTACK	= 1.5f;

		internal Signal<int> hitByPlayerSignal = new Signal<int> ();
		internal Signal enterCollisionSignal = new Signal ();
		internal Signal exitCollisionSignal = new Signal ();

		public float moveSpeed = 2f;			// The speed the enemy moves at.

		private Vector3 _hpScale;
		private Transform _hpTransform;
		private Transform _hpBgTransform;

		private bool _canMove = true;
		private bool _isPlayerTrigger = false;
		private bool _isEnemyTrigger = true;
		private bool _isWait = false;
		private float _waitTime = 0f;
		public bool canHit { get; private set; }

		private Rigidbody2D _rigidBody;
		private BoxCollider2D _collider2d;
		private float _speed = 0;

		private EnemyStates _state;

		public override void init()
		{
			// Setting up the references.
			_frontCheck = transform.Find(FRONT_CHECK).transform;
			_explosion = transform.Find(EXPLOSION).transform;
			_hpTransform = transform.Find (HEALTH).transform;
			_hpBgTransform = transform.Find (HEALTH_BG).transform;
			_hpScale = _hpTransform.localScale;

			_rigidBody = GetComponent<Rigidbody2D> ();
			_collider2d = GetComponent<BoxCollider2D> ();
			canHit = true;

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

			if (canHit && _isPlayerTrigger && _speed == 0) {
				setState(EnemyStates.HIT);
			}
		}

		void OnTriggerEnter2D(Collider2D other)
		{
			Debug.Log ("OnTriggerEnter2D. name = " + other.gameObject.name + ", state = " + _state.ToString());
			string otherName = other.gameObject.name;
			if (otherName == WALL_LEFT || otherName == WALL_RIGHT) {
				flip ();
			}
			else if (isOtherEnemy (other) && isEqualsScaleX(other.gameObject)) {
				_isEnemyTrigger = true;
				setState (EnemyStates.IDLE);
				setState (EnemyStates.BEFORE_ENEMY);
			}
			else if (isPlayerObject(other.tag)) {
				if (canHit) {
					setState (EnemyStates.IDLE);
					setState (EnemyStates.HIT);
				} else if (!_isWait) {
					setState (EnemyStates.WAIT_TO_HIT);
				}
			}
			//Debug.Log("Something has entered this zone.");    
		}  

		void OnTriggerStay2D(Collider2D other)
		{
			Debug.Log ("OnTriggerStay2D. name = " + other.gameObject.name + ", state = " + _state.ToString());
			if (_speed == 0 && !_isWait) {
				if (isPlayerObject (other.tag) && _isPlayerTrigger) {
					float dist1 = other.bounds.SqrDistance (_collider2d.bounds.center);
					float width = (other as BoxCollider2D).size.x + _collider2d.size.x;
					if (width * 0.4f > dist1 || dist1 > width*1.4f) {
						_isPlayerTrigger = false;
						exitCollisionSignal.Dispatch ();
						startWait (waitToMove(), TIME_TO_MOVE);
					}
				}
				/*else if (isEqualsNames(other.gameObject) && _state == EnemyStates.BEFORE_ENEMY) {
					//playMove ();
				}*/
			}
		}

		void OnTriggerExit2D(Collider2D other)
		{
			Debug.Log ("OnTriggerExit2D. name = " + other.gameObject.name + ", _speed = " + _speed + ", state = " + _state.ToString ());
			if (isPlayerObject (other.tag)) {
				_isPlayerTrigger = false;
				setState(EnemyStates.MOVE);
			} else if (isOtherEnemy (other)) {
				_isEnemyTrigger = false;
				setState(EnemyStates.MOVE);
			}
		}

		IEnumerator waitToMove()
		{
			yield return new WaitForSeconds (_waitTime);
			_isWait = false;
			if (_isEnemyTrigger) {
				startWait (waitToMove(), TIME_TO_MOVE);
			} else {
				setState(EnemyStates.MOVE);
			}
		}

		IEnumerator waitToHit()
		{
			yield return new WaitForSeconds (_waitTime);
			_isWait = false;
			if (_isPlayerTrigger && canHit) {
				setState (EnemyStates.HIT);
			} else {
				setState (EnemyStates.MOVE);
			}
		}

		IEnumerator onAttackComplete () 
		{
			yield return new WaitForSeconds (_waitTime);
			_isWait = false;
			setState(EnemyStates.WAIT_TO_HIT);
		}

		private void startWait(IEnumerator routine, float time)
		{
			if (!_isWait) {
				_waitTime = time;
				_isWait = true;
				StartCoroutine (routine);
			}
		}

		private void setState(EnemyStates value, bool isForce=false)
		{
			if (_state != value || isForce) {
				switch (value) {
				case EnemyStates.MOVE:
					if (_isPlayerTrigger || _state == EnemyStates.NULL) {
						playMove ();
					}
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
					startWait (waitToHit(), TIME_TO_HIT);
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
				if (_isWait || _isPlayerTrigger) {
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
				stopMove ();
				playAnimation (EnemyAnimatorTypes.TRIGGER_DEFEAT);
			}
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

		private bool isEqualsNames(GameObject go)
		{
			return gameObject.name.Equals (go.name);
		}

		private bool isEqualsScaleX(GameObject go)
		{
			return gameObject.transform.localScale.x == go.transform.localScale.x;
		}

		private bool isPlayerObject(string tag)
		{
			return (tag == PlayerView.ID);
		}

		private bool isOtherEnemy(Collider2D other)
		{
			return (other.tag == EnemyView.ID && !isEqualsNames(other.gameObject));
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
				exitCollisionSignal.Dispatch ();
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
			/*if (_rigidBody != null) {
				_rigidBody.isKinematic = true;
			}

			if (_collider2d != null) {
				_collider2d.enabled = false;
			}*/
		}
	}
}
