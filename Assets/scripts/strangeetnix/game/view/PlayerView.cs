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

		private float TIME_TO_ATTACK	= 1.5f;
		private float TIME_TO_DEFEAT	= 1.5f;

		internal Signal hitEnemySignal = new Signal ();

		internal bool facingRight = true;			// For determining which way the player is currently facing.
		internal bool canHit = true;

		public float moveForce = 200f;			// Amount of force added to move the player left and right.
		public float maxSpeed = 0.8f;			// The fastest the player can travel in the x axis.
		//public float jumpForce = 100f;		// Amount of force added when the player jumps.

		private Transform _explosion2;

		private int _hitNum = 0;

		private float _moveSpeed = 0f;

		private bool _pressedButton = false;
		private bool _battleMode;

		private CharacterStates _state;

		public override void init (bool battleMode)
		{
			_battleMode = battleMode;
			if (_battleMode) {
				_explosion = transform.Find (EXPLOSION1).transform;
				_explosion2 = transform.Find (EXPLOSION2).transform;
			}

			_state = CharacterStates.IDLE;

			base.init (battleMode);

			int battleVal = (_battleMode) ? 1 : 0;
			_anim.SetInteger(PlayerAnimatorTypes.INT_BATTLE, battleVal);
		}

		void FixedUpdate ()
		{
			bool isEnemy = (_battleMode && isHit);

			// Cache the horizontal input.
			float h = _moveSpeed;//Input.GetAxis("Horizontal");

			// The Speed animator parameter is set to the absolute value of the horizontal input.
			float speed = Mathf.Abs(h);

			if (isEnemy) {// || isHit || isPlayAnimation(PlayerAnimatorTypes.TRIGGER_DEFEAT)) {
				h = speed = 0;
			}

			_anim.SetFloat (PlayerAnimatorTypes.FLOAT_SPEED, speed);

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

		internal Vector2 explosionPos
		{
			get { 
				Vector3 pos = (_hitNum == 1) ? _explosion.position : _explosion2.position;
				return new Vector2 (pos.x, pos.y); 
			}
		}

		internal void stopWalk(bool pressedButton=false)
		{
			_pressedButton = pressedButton;
			if (_moveSpeed != 0 && !_pressedButton) {
				_moveSpeed = 0;
				setState (CharacterStates.IDLE);
			}
		}

		internal void startLeftWalk(bool pressedButton=false)
		{
			if (!_isDead && !_isWait) {			
				_pressedButton = pressedButton;
				_moveSpeed = -0.5f;

				setState (CharacterStates.MOVE, true);
			}
		}

		internal void startRightWalk(bool pressedButton=false)
		{
			if (!_isDead && !_isWait) {
				_pressedButton = pressedButton;
				_moveSpeed = 0.5f;

				setState (CharacterStates.MOVE, true);
			}
		}

		internal void startHit1()
		{
			if (!_isDead && !isHit && canHit) {
				_pressedButton = false;
				_hitNum = 1;
				setState (CharacterStates.HIT, true);
			}
		}

		internal void startHit2()
		{
			if (!_isDead && !isHit && canHit) {
				_pressedButton = false;
				_hitNum = 2;
				setState (CharacterStates.HIT, true);
			}
		}

		internal bool isHit
		{
			get { return _state == CharacterStates.HIT; }
		}

		IEnumerator onWaitComplete () 
		{
			yield return new WaitForSeconds (_waitTime);
			_isWait = false;
			_state = CharacterStates.IDLE;
		}

		override public void setDeath()
		{
			if (!_isDead) {
				setState (CharacterStates.DEATH);
			}
		}

		internal void checkToFlip (Transform enemyTransform)
		{
			if (!_isWait) {
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
			if (!_isWait) {
				setState (CharacterStates.DEFEAT);
			}
		}

		public void setState(CharacterStates state, bool isForce=false)
		{
			string animationType;
			Debug.Log ("PlayerView. state = " + state.ToString());
			if (_state != state || isForce) {
				if (_isWait) {
					StopCoroutine (onWaitComplete());
					onWaitComplete ();
				}
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
					animationType = (_hitNum == 1) ? PlayerAnimatorTypes.TRIGGER_HIT : PlayerAnimatorTypes.TRIGGER_SUPER_HIT;
					playAnimation (animationType);
					startWait (onWaitComplete (), TIME_TO_ATTACK);
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
					if (_moveSpeed > 0) {
						stopWalk ();
					}

					startWait (onWaitComplete(), TIME_TO_DEFEAT);
					playAnimation (PlayerAnimatorTypes.TRIGGER_DEFEAT);
					break;
				}
			}
		}
	}
}
