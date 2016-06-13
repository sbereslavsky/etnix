//The "View" for the player's ship. This MonoBehaviour is attached to the player_ship prefab inside Unity.

using System;
using System.Collections.Generic;
using UnityEngine;

using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;

namespace strangeetnix.game
{
	public class PlayerView : View
	{
		public static string ID = "Player";

		//private int TAG_WALK 			= Animator.StringToHash("walk");
		private int TAG_DEFEAT 			= Animator.StringToHash("defeat");
		private int TAG_HIT 			= Animator.StringToHash("hit");
		private int TAG_SUPER_HIT 		= Animator.StringToHash("superHit");

		// Setting up references.
		private string FRONT_CHECK 	= "frontCheck";
		private string EXPLOSION1 	= "explosion1";
		private string EXPLOSION2 	= "explosion2";

		internal Signal<GameObject> hitEnemySignal = new Signal<GameObject> ();

		[HideInInspector]
		public bool facingRight = true;			// For determining which way the player is currently facing.

		public float moveForce = 200f;			// Amount of force added to move the player left and right.
		public float maxSpeed = 0.8f;			// The fastest the player can travel in the x axis.
		//public float jumpForce = 100f;		// Amount of force added when the player jumps.

		private Transform _frontCheck;			// Reference to the position of the gameobject used for checking if something is in front.
		private Animator anim;  				// Reference to the player's animator component.
		private Transform _explosion1;
		private Transform _explosion2;

		private bool _isDead;					// Whether or not the enemy is dead.
		private string _hitEnemyName;

		private string _state = "";
		private float _moveSpeed = 0f;

		private bool _pressedButton = false;

		private int _hitNum = 0;
		internal bool canHit = true;

		private bool _battleMode;

		internal void init(bool battleMode)
		{
			_isDead = false;
			_battleMode = battleMode;
			if (_battleMode) {
				_hitEnemyName = "";
				_explosion1 = transform.Find (EXPLOSION1).transform;
				_explosion2 = transform.Find (EXPLOSION2).transform;
				_frontCheck = transform.Find (FRONT_CHECK).transform;
			} 

			int battleVal = (_battleMode) ? 1 : 0;
			anim = GetComponent<Animator> ();
			anim.SetInteger(PlayerAnimatorTypes.INT_BATTLE, battleVal);
		}

		void FixedUpdate ()
		{
			bool startHit1 = isHit;
			if (!startHit1) {
				setState ();
			}

			bool isEnemy = false;
			if (_battleMode) {
				// Create an array of all the colliders in front of the enemy.
				Collider2D[] frontHits = Physics2D.OverlapPointAll(_frontCheck.position);

				// Check each of the colliders.
				foreach(Collider2D c in frontHits)
				{
					// If any of the colliders is an Obstacle...
					isEnemy = c.tag.Contains(EnemyView.ID);
					if (startHit1 && isEnemy && _hitEnemyName.Length == 0 && c.gameObject.name != _hitEnemyName)
					{
						if (gameObject.transform.localScale.x > 0 && c.gameObject.transform.localScale.x > 0 ||
							gameObject.transform.localScale.x < 0 && c.gameObject.transform.localScale.x < 0) {
							_hitEnemyName = c.gameObject.name;
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

			if (isEnemy) {// || isHit || isPlayAnimation(TAG_DEFEAT)) {
				h = speed = 0;
			}

			//_moveSpeed = 0;

			//print ("speed = " + speed);
			//if (anim.GetFloat (PlayerAnimatorTypes.FLOAT_SPEED) != speed) {
				anim.SetFloat (PlayerAnimatorTypes.FLOAT_SPEED, speed);
			//}

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
				Flip();
			// Otherwise if the input is moving the player left and the player is facing right...
			else if(h < 0 && facingRight)
				// ... flip the player.
				Flip();
		}

		private void postHitEnemy(GameObject enemyGO)
		{
			hitEnemySignal.Dispatch (enemyGO);
		}

		internal void playDefeatAnimation()
		{
			if (anim && !isHit && !isPlayAnimation(TAG_DEFEAT))
			{
				anim.SetTrigger(PlayerAnimatorTypes.TRIGGER_DEFEAT);
			}
		}

		internal void playIdleAnimation()
		{
			if (anim)
			{
				anim.SetTrigger(PlayerAnimatorTypes.TRIGGER_IDLE);
			}
		}

		internal Vector2 explosionPos
		{
			get { 
				Vector3 pos = (_hitNum == 1) ? _explosion1.position : _explosion2.position;
				return new Vector2 (pos.x, pos.y); 
			}
		}

		//Set the IME value
		internal void SetAction(int evt)
		{
			int input = evt;
			bool left = (input & GameInputEvent.LEFT) > 0;
			bool right = (input & GameInputEvent.RIGHT) > 0;
			bool hit = (input & GameInputEvent.HIT) > 0;
			if (input == 0) {
				stopWalk ();
			}

			if (left)
			{
				startLeftWalk ();
			}

			if (right)
			{
				startRightWalk ();
			}

			if (hit)
			{
				startHit1 ();
				//startHit2 ();
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
					Flip ();
				}
			}
		}

		internal void startRightWalk(bool pressedButton=false)
		{
			if (!_isDead) {
				_pressedButton = pressedButton;
				_moveSpeed = 0.5f;

				if (!facingRight) {
					Flip ();
				}
			}
		}

		internal void startHit1()
		{
			if (!_isDead && !isHit && canHit) {
				_pressedButton = false;
				_hitNum = 1;
				startHit (PlayerAnimatorTypes.TRIGGER_HIT);
			}
		}

		internal void startHit2()
		{
			if (!_isDead && !isHit && canHit) {
				_pressedButton = false;
				_hitNum = 2;
				startHit (PlayerAnimatorTypes.TRIGGER_SUPER_HIT);
			}
		}

		private void startHit(string triggerState)
		{
			if (_state != triggerState) {
				setState (triggerState);
				anim.SetTrigger (triggerState);

				_hitEnemyName = "";
			}
		}

		internal bool isHit
		{
			get { return isPlayAnimation (TAG_HIT) || isPlayAnimation (TAG_SUPER_HIT); }
		}

		internal void setDeath()
		{
			if (!_isDead) {
				// Set dead to true.
				_isDead = true;

				if (anim) {
					anim.SetTrigger (PlayerAnimatorTypes.TRIGGER_DEATH1);
				}

				// Find all of the sprite renderers on this object and it's children.
				/*SpriteRenderer[] otherRenderers = GetComponentsInChildren<SpriteRenderer>();

				// Disable all of them sprite renderers.
				foreach(SpriteRenderer s in otherRenderers)
				{
					s.enabled = false;
				}*/
			}
		}

		internal void checkToFlip (Transform enemyTransform)
		{
			if (!isHit || !isPlayAnimation(TAG_DEFEAT)) {
				if (enemyTransform.position.x < gameObject.transform.position.x && facingRight ||
				    enemyTransform.position.x > gameObject.transform.position.x && !facingRight) {
					Flip ();
				}
			}
		}

		internal void Flip ()
		{
			// Switch the way the player is labelled as facing.
			facingRight = !facingRight;

			// Multiply the player's x local scale by -1.
			Vector3 theScale = transform.localScale;
			theScale.x *= -1;
			transform.localScale = theScale;
		}

		private void setState(string value = "")
		{
			_state = value;
		}

		private bool isPlayAnimation(int tagHash)
		{
			AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo (0);
			return stateInfo.tagHash == tagHash;
		}
	}
}
