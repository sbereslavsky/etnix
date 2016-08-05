//The "View" for an Enemy. This MonoBehaviour is attached to the enemy prefab inside Unity.

using System;
using UnityEngine;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;
using strange.extensions.pool.api;
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

		internal string poolKey{ get; set; }

		internal Signal<GameObject> triggerEnterSignal = new Signal<GameObject> ();
		internal Signal<GameObject> triggerExitSignal = new Signal<GameObject> ();

		internal Signal<bool> forceExitTriggerSignal = new Signal<bool> ();
		internal Signal checkTriggerSignal = new Signal();

		internal Signal<int> hitEnemySignal = new Signal<int> ();
		internal Signal hitPlayerSignal = new Signal ();

		public float moveSpeed = 2f;			// The speed the enemy moves at.
		public bool canHit { get; set; }
		public bool isMove { get { return (_speed > 0); }}
		public CharacterStates currentState { get { return _state; } }

		private Vector3 _hpScale;
		private Transform _hpTransform;
		private Transform _hpBgTransform;

		private bool _canMove;

		private float _speed = 0;

		private CharacterStates _state;

		private float _hpScaleX;

		internal BoxCollider2D boxCollider { get; private set; }

		public override void init()
		{
			// Setting up the references.
			_explosion = transform.Find(EXPLOSION).transform;
			_hpTransform = transform.Find (HEALTH).transform;
			_hpBgTransform = transform.Find (HEALTH_BG).transform;
			_hpScale = _hpTransform.localScale;
			boxCollider = gameObject.GetComponent<BoxCollider2D> ();
			_canMove = true;
			canHit = true;

			_hpScaleX = _hpTransform.localScale.x;

			base.init ();
			setState (CharacterStates.IDLE);

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
			
		internal void disableCanMove() 
		{
			_canMove = false;
		}

		internal void destroyComponent()
		{
			Destroy (this);
			_hpTransform.localScale = new Vector3(_hpScaleX, _hpScale.y, 1);
			_hpBgTransform.localScale = new Vector3(_hpScaleX, _hpScale.y, 1);
		}

		void OnTriggerEnter2D(Collider2D other)
		{
			string otherName = other.gameObject.name;
			if (otherName == WALL_LEFT || otherName == WALL_RIGHT) {
				flip ();
			}
			else { 
				Debug.Log ("OnTriggerEnter2D. other.tag = " + other.tag + ", name = " + other.gameObject.name);
				bool isPlayer = isPlayerObject (other.tag);
				if (this.enabled && (isPlayer || (isOtherEnemy (other) && isEqualsScaleX (other.gameObject)))) {
					GameObject otherGO = (isPlayer) ? null : other.gameObject;
					triggerEnterSignal.Dispatch (otherGO);
				}
			}  
		}  

		void OnTriggerStay2D(Collider2D other)
		{
			if (_speed == 0 && canHit && isPlayerObject (other.tag)) {
				if (isCollisionOut (other)) {
					forceExitTriggerSignal.Dispatch (true);
				}
			}
		}

		void OnTriggerExit2D(Collider2D other)
		{
			bool isPlayer = isPlayerObject (other.tag);
			if (isPlayer || isOtherEnemy (other)) {
				triggerExitSignal.Dispatch (other.gameObject);
			}
		}

		internal void setState(CharacterStates value, bool isForce=false)
		{
			if (_state != value || isForce) {
				switch (value) {
				case CharacterStates.MOVE:
					playMove ();
					break;

				case CharacterStates.HIT:
					playAnimation (EnemyAnimatorTypes.TRIGGER_HIT);
					hitPlayerSignal.Dispatch();
					break;

				case CharacterStates.DEFEAT:
					setState (CharacterStates.IDLE);
					playAnimation (EnemyAnimatorTypes.TRIGGER_DEFEAT);
					break;

				case CharacterStates.DEATH:
					setDeath ();
					setState (CharacterStates.IDLE);
					playAnimation (EnemyAnimatorTypes.TRIGGER_DEATH);
					break;

				case CharacterStates.IDLE:
					stopMove ();
					playAnimation (EnemyAnimatorTypes.TRIGGER_IDLE);
					break;
				}

				_state = value;
			}
		}

		void FixedUpdate ()
		{
			if (_canMove && _state == CharacterStates.MOVE && _state != CharacterStates.DEFEAT) {
				// Set the enemy's velocity to moveSpeed in the x direction.
				if (_speed > 0) {
					_rigidBody.velocity = new Vector2(-transform.localScale.x * moveSpeed, _rigidBody.velocity.y);	
				}
			}
		}

		internal bool canToDefeat()
		{
			// Reduce the number of hit points by one.
			return (_anim && _state != CharacterStates.HIT && _state != CharacterStates.DEFEAT);
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
				if (_state == CharacterStates.HIT || _state == CharacterStates.BEFORE_ENEMY) {
					forceExitTriggerSignal.Dispatch (_state == CharacterStates.HIT);
					_state = CharacterStates.MOVE;
				}
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

		internal void updateHp(int currentHp, int startHp)
		{
			if (startHp > 0) {
				int hpKoef = 100 * currentHp / startHp;
				_hpTransform.localScale = new Vector3(_hpScale.x * hpKoef * 0.01f, _hpScale.y, 1);
				if (hpKoef == 0) {
					_hpBgTransform.localScale = new Vector3(0, _hpScale.y, 1);
				}
			} else {
				Debug.LogError ("updateEnemyHp.startHP = 0!");
			}
		}
	}
}
