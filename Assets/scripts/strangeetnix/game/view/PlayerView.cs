//The "View" for the player's ship. This MonoBehaviour is attached to the player_ship prefab inside Unity.

using System;
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
		private string FRONT_CHECK 	= "frontCheck";
		private string EXPLOSION1 	= "explosion1";
		private string EXPLOSION2 	= "explosion2";

		internal Signal<GameObject> hitEnemySignal = new Signal<GameObject> ();

		internal bool facingRight = true;			// For determining which way the player is currently facing.
		internal bool canHit = true;

		public float moveForce = 200f;			// Amount of force added to move the player left and right.
		public float maxSpeed = 0.8f;			// The fastest the player can travel in the x axis.
		//public float jumpForce = 100f;		// Amount of force added when the player jumps.

		private Transform _explosion2;

		private int _hitNum = 0;
		private string _hitEnemyName;

		private float _moveSpeed = 0f;

		private bool _pressedButton = false;
		private bool _battleMode;

		public override void init (bool battleMode)
		{
			_battleMode = battleMode;
			if (_battleMode) {
				_hitEnemyName = "";
				_explosion = transform.Find (EXPLOSION1).transform;
				_explosion2 = transform.Find (EXPLOSION2).transform;
				_frontCheck = transform.Find (FRONT_CHECK).transform;
			}

			base.init (battleMode);

			int battleVal = (_battleMode) ? 1 : 0;
			_anim.SetInteger(PlayerAnimatorTypes.INT_BATTLE, battleVal);
		}

		void OnTriggerEnter2D(Collider2D other)
		{
			Debug.Log("Something has entered this zone.");    
		} 

		void OnTriggerStay2D(Collider2D other)
		{
			Debug.Log("Something has entered this zone.");    
		} 

		void OnTriggerExit2D(Collider2D other)
		{
			Debug.Log("Something has entered this zone.");    
		}

		void FixedUpdate ()
		{
			bool isEnemy = false;
			if (_battleMode && isHit && _hitEnemyName.Length == 0) {
				// Create an array of all the colliders in front of the enemy.
				Collider2D[] frontHits = Physics2D.OverlapPointAll(_frontCheck.position);

				// Check each of the colliders.
				foreach(Collider2D c in frontHits)
				{
					// If any of the colliders is an Obstacle...
					isEnemy = c.tag.Contains(EnemyView.ID);
					if (isEnemy && c.gameObject.name != _hitEnemyName)
					{
						if (gameObject.transform.localScale.x > 0 && c.gameObject.transform.localScale.x > 0 ||
							gameObject.transform.localScale.x < 0 && c.gameObject.transform.localScale.x < 0) {
							postHitEnemy (c.gameObject);
							break;
						}
					}
				}
			}

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

		private void postHitEnemy(GameObject enemyGO)
		{
			_hitEnemyName = enemyGO.name;
			hitEnemySignal.Dispatch (enemyGO);
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
			}
		}

		internal void startLeftWalk(bool pressedButton=false)
		{
			if (!_isDead) {			
				_pressedButton = pressedButton;
				_moveSpeed = -0.5f;

				if (facingRight) {
					flip ();
				}
			}
		}

		internal void startRightWalk(bool pressedButton=false)
		{
			if (!_isDead) {
				_pressedButton = pressedButton;
				_moveSpeed = 0.5f;

				if (!facingRight) {
					flip ();
				}
			}
		}

		internal void startHit1()
		{
			if (!_isDead && !isHit && canHit) {
				_pressedButton = false;
				_hitNum = 1;
				playAnimation (PlayerAnimatorTypes.TRIGGER_HIT);
				_hitEnemyName = "";
			}
		}

		internal void startHit2()
		{
			if (!_isDead && !isHit && canHit) {
				_pressedButton = false;
				_hitNum = 2;
				playAnimation (PlayerAnimatorTypes.TRIGGER_SUPER_HIT);
				_hitEnemyName = "";
			}
		}

		internal bool isHit
		{
			get { return isPlayAnimation (PlayerAnimatorTypes.TRIGGER_HIT) || isPlayAnimation (PlayerAnimatorTypes.TRIGGER_SUPER_HIT); }
		}

		override public void setDeath()
		{
			if (!_isDead) {
				// Set dead to true.
				_isDead = true;

				playAnimation (PlayerAnimatorTypes.TRIGGER_DEATH1);
			}
		}

		internal void checkToFlip (Transform enemyTransform)
		{
			if (!isHit || !isPlayAnimation(PlayerAnimatorTypes.TRIGGER_DEFEAT)) {
				if (enemyTransform.position.x < gameObject.transform.position.x && facingRight ||
				    enemyTransform.position.x > gameObject.transform.position.x && !facingRight) {
					flip ();
				}
			}
		}

		protected override void flip ()
		{
			// Switch the way the player is labelled as facing.
			facingRight = !facingRight;

			base.flip ();
		}

		internal void playDefeatAnimation()
		{
			if (!isPlayAnimation(PlayerAnimatorTypes.TRIGGER_DEFEAT) && !isHit)
			{
				playAnimation (PlayerAnimatorTypes.TRIGGER_DEFEAT);
			}
		}
	}
}
