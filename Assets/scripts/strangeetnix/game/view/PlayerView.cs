//The "View" for the player's ship. This MonoBehaviour is attached to the player_ship prefab inside Unity.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;

namespace strangeetnix.game
{
	public class PlayerView : CharacterView
	{
		public static string ID = "Player";

		// Setting up references.
		private string EXPLOSION1 	= "explosion1";
		private string EXPLOSION2 	= "explosion2";

		internal Signal hitEnemySignal = new Signal ();
		internal Signal stopWalkSignal = new Signal ();

		internal bool facingRight = true;		// For determining which way the player is currently facing.
		internal bool canHit = true;

		public float moveForce = 200f;			// Amount of force added to move the player left and right.
		public float maxSpeed = 0.8f;			// The fastest the player can travel in the x axis.
		//public float jumpForce = 100f;		// Amount of force added when the player jumps.

		private Transform _explosion2;

		private int _hitNum = 0;
		private string _hitType = "";

		private float _moveSpeed = 0f;

		private CharacterStates _state;

		internal int id { get; set; }
		internal bool battleMode { get; set; }

		public override void init ()
		{
			if (battleMode) {
				_explosion = transform.Find (EXPLOSION1).transform;
				_explosion2 = transform.Find (EXPLOSION2).transform;
			}

			_state = CharacterStates.IDLE;

			base.init ();

			int battleVal = (battleMode) ? 1 : 0;
			_animator.SetInteger(PlayerAnimatorTypes.INT_BATTLE, battleVal);
		}

		internal Vector2 explosionPos 
		{
			get { 
				Vector3 pos = _explosion.position;
				if (id == 1) {
					pos = (_hitNum != 1) ? _explosion.position : _explosion2.position;
				}
				else if (_hitNum == 0) {
					pos = _explosion2.position;
				}
				return new Vector2 (pos.x, pos.y); 
			}
		}

		internal bool isStop { get { return _moveSpeed == 0;}}

		internal bool canStartMove { get { return !_isDead && !isHit && !isDefeat;}}
		internal bool canStartHit { get { return !_isDead && !isHit && canHit;}}

		internal bool isHit { get { return (_hitType.Length > 0 && isPlayAnimation(_hitType)); }}
		private bool isDefeat { get { return isPlayAnimation(PlayerAnimatorTypes.TRIGGER_DEFEAT);}}

		void FixedUpdate ()
		{			
			// Cache the horizontal input.
			float h = _moveSpeed;//Input.GetAxis("Horizontal");

			// The Speed animator parameter is set to the absolute value of the horizontal input.
			float speed = Mathf.Abs(h);

			if (battleMode && (isHit || isDefeat)) {// || isHit || isPlayAnimation(PlayerAnimatorTypes.TRIGGER_DEFEAT)) {
				h = speed = 0;
			}

			_animator.SetFloat (PlayerAnimatorTypes.FLOAT_SPEED, speed);

			Rigidbody2D rigidBody = GetComponent<Rigidbody2D> ();
			// If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeed yet...
			if(h * rigidBody.velocity.x < maxSpeed)
				// ... add a force to the player.
				rigidBody.AddForce(Vector2.right * h * moveForce);

			// If the player's horizontal velocity is greater than the maxSpeed...
			if(Mathf.Abs(rigidBody.velocity.x) > maxSpeed)
				// ... set the player's velocity to the maxSpeed in the x axis.
				rigidBody.velocity = new Vector2(Mathf.Sign(rigidBody.velocity.x) * maxSpeed, rigidBody.velocity.y);

			// If the input is moving the player right and the player is facing left...
			if(h > 0 && !facingRight)
				// ... flip the player.
				flip();
			// Otherwise if the input is moving the player left and the player is facing right...
			else if(h < 0 && facingRight)
				// ... flip the player.
				flip();
		}

		override public void setDeath()
		{
			if (!_isDead) {
				setState (CharacterStates.DEATH);
			}
		}

		internal void checkToFlip (Transform enemyTransform)
		{
			if (!isDefeat && !isHit) {
				if (enemyTransform.position.x < gameObject.transform.position.x && facingRight ||
				    enemyTransform.position.x > gameObject.transform.position.x && !facingRight) {
					flip ();
				}
			}
		}

		public override void flip ()
		{
			// Switch the way the player is labelled as facing.
			facingRight = !facingRight;

			base.flip ();
		}

		internal void playDefeatAnimation()
		{
			if (!isDefeat && !isHit) {
				setState (CharacterStates.DEFEAT);
			}
		}

		internal void setMoveSpeed(float value)
		{
			_moveSpeed = value;
		}

		internal void setHitId(int id)
		{
			_hitNum = id;
		}

		public void setState(CharacterStates state, bool isForce=false)
		{
			string animationType;
			Debug.Log ("PlayerView. state = " + state.ToString());
			if (_state != state || isForce) {
				_state = state;

				switch (state) {
				case CharacterStates.MOVE:
					if (_moveSpeed > 0) {
						if (!facingRight) {
							flip ();
						}
					} else {
						if (facingRight) {
							flip ();
						}
					}
					break;

				case CharacterStates.HIT:
					_hitType = getAnimationHitType();
					playAnimation (_hitType);
					hitEnemySignal.Dispatch ();
					break;

				case CharacterStates.IDLE:
					break;

				case CharacterStates.DEATH:
					// Set dead to true.
					_isDead = true;
					animationType = (UnityEngine.Random.Range (1, 1000) % 2 == 0) ? PlayerAnimatorTypes.TRIGGER_DEATH1 : PlayerAnimatorTypes.TRIGGER_DEATH2;
					playAnimation (animationType);
					break;

				case CharacterStates.DEFEAT:
					if (!isStop) {
						stopWalkSignal.Dispatch ();
						//stopWalk ();
					}

					playAnimation (PlayerAnimatorTypes.TRIGGER_DEFEAT);
					break;
				}
			}
		}

		private string getAnimationHitType()
		{
			string animationType;
			switch (_hitNum) {
			case 1:
				animationType = PlayerAnimatorTypes.TRIGGER_HIT;
				break;
			case 2:
				animationType = PlayerAnimatorTypes.TRIGGER_HIT2;
				break;
			default:
				animationType = PlayerAnimatorTypes.TRIGGER_HIT3;
				break;
			}

			return animationType;
		}
	}
}
