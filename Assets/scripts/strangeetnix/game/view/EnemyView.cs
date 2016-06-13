//The "View" for an Enemy. This MonoBehaviour is attached to the enemy prefab inside Unity.

using System;
using UnityEngine;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;
using System.Collections;

namespace strangeetnix.game
{
	public class EnemyView : View
	{
		public static string ID = "Enemy";

		private string FRONT_CHECK 	= "frontCheck";
		private string HEALTH		= "health";
		private string HEALTH_BG	= "healthBg";

		//private int TAG_IDLE = Animator.StringToHash("idle");
		//private int TAG_WALK = Animator.StringToHash("walk");			
		private int TAG_DEFEAT = Animator.StringToHash("defeat");
		private int TAG_HIT = Animator.StringToHash("hit");

		internal Signal collisionSignal = new Signal ();

		public float moveSpeed = 2f;			// The speed the enemy moves at.

		private Transform _frontCheck;			// Reference to the position of the gameobject used for checking if something is in front.
		private Transform _explosionTransform;

		private Vector3 _hpScale;
		private Transform _hpTransform;
		private Transform _hpBgTransform;

		private Animator anim;
		private bool _isIdle = false;
		private bool _isMove = false;
		private bool _isCollider = false;

		private bool _canMove = true;

		internal bool canHit = true;

		internal void init()
		{
			// Setting up the references.

			_frontCheck = transform.Find(FRONT_CHECK).transform;
			_explosionTransform = transform.Find("explosion").transform;
			_hpTransform = transform.Find (HEALTH).transform;
			_hpBgTransform = transform.Find (HEALTH_BG).transform;
			_hpScale = _hpTransform.localScale;
			//score = GameObject.Find("Score").GetComponent<Score>();
			anim = GetComponent<Animator>();
			//InvokeRepeating("playWalkAnimation", 0.3f, 0f);

			checkToFlip ();
		}

		internal Vector2 getExplosionPos()
		{
			return new Vector2 (_explosionTransform.position.x, _explosionTransform.position.y);
		}

		internal void Flip()
		{
			// Multiply the x component of localScale by -1.
			Vector3 enemyScale = transform.localScale;
			enemyScale.x *= -1;
			transform.localScale = enemyScale;
		}

		private void checkToFlip()
		{
			GameObject playerGO = GameObject.FindGameObjectWithTag (PlayerView.ID);
			if (playerGO) {
				Transform player = playerGO.transform;
				if (player.position.x > transform.position.x) {
					Flip ();
				}
			}
		}

		internal void dispose() 
		{
			//костыль, не удаляется анимация и медиатор врага со сцены
			if (anim) {
				anim.Stop ();
			}

			gameObject.SetActive (false);
			Destroy (gameObject.GetComponent<BoxCollider2D> ());
		}
			
		internal void SetCanMove(bool value) 
		{
			_canMove = value;
			/*if (_canMove) {
				checkToFlip ();
			}*/
		}

		void FixedUpdate ()
		{
			if (_canMove && !isPlayAnimation (TAG_DEFEAT)) {
				// Create an array of all the colliders in front of the enemy.
				Collider2D[] frontHits = Physics2D.OverlapPointAll(_frontCheck.position);
				//Collider2D[] frontHits = Physics2D.OverlapPointAll(transform.position, 1);

				_isCollider = false;
				// Check each of the colliders.
				foreach(Collider2D c in frontHits)
				{
					bool isPlayer = c.tag == PlayerView.ID;
					bool isEnemy = c.tag == ID && !this.gameObject.Equals(c.gameObject);
					// If any of the colliders is an Obstacle...
					if (isPlayer || isEnemy) {
						if (isPlayer && c.gameObject != null && canHit) {
							playHitAnimation ();
							collisionSignal.Dispatch();
						} else if (isEnemy && !_isIdle) {
							_isIdle = true;
							playIdleAnimation ();
						}
						_isMove = false;
						return;
					}
				}

				if (!_isCollider && !_isMove) {
					_isMove = true;
					_isIdle = false;
					playWalkAnimation ();
				}

				// Set the enemy's velocity to moveSpeed in the x direction.
				if (_isMove) {
					GetComponent<Rigidbody2D>().velocity = new Vector2(-transform.localScale.x * moveSpeed, GetComponent<Rigidbody2D>().velocity.y);	
				}

				// If the enemy has one hit point left and has a damagedEnemy sprite...
				/*if(HP == 1 && damagedEnemy != null)
				// ... set the sprite renderer's sprite to be the damagedEnemy sprite.
				ren.sprite = damagedEnemy;*/
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

		internal void hitByPlayer()
		{
			// Reduce the number of hit points by one.
			if (anim && !isPlayAnimation(TAG_DEFEAT) && !isPlayAnimation(TAG_HIT)) {
				_isMove = _isIdle = false; 
				playDefeatAnimation();
			}
		}

		internal void setDeath()
		{
			this.enabled = false;
			playDeathAnimation ();
			Rigidbody2D rigidbody2D = this.GetComponent<Rigidbody2D> ();
			if (rigidbody2D != null) {
				rigidbody2D.isKinematic = true;
			}

			BoxCollider2D boxCollider2D = this.GetComponent<BoxCollider2D> ();
			if (boxCollider2D != null) {
				boxCollider2D.enabled = false;
			}
		}

		internal void DestroyView(float time)
		{
			Destroy (this.gameObject, time);
		}

		internal void playWalkAnimation()
		{
			playAnimation (EnemyAnimatorTypes.TRIGGER_WALK);
		}

		internal void playIdleAnimation()
		{
			playAnimation (EnemyAnimatorTypes.TRIGGER_IDLE);
		}

		internal void playHitAnimation()
		{
			playAnimation (EnemyAnimatorTypes.TRIGGER_HIT);
		}

		private void playDeathAnimation()
		{
			playAnimation (EnemyAnimatorTypes.TRIGGER_DEATH);
		}

		internal void playDefeatAnimation()
		{
			playAnimation (EnemyAnimatorTypes.TRIGGER_DEFEAT);
		}

		private void playAnimation(string triggerName)
		{
			if (anim) {
				anim.SetTrigger (triggerName);
			} else {
				Debug.Log ("playAnimation(" + triggerName + "). anim = null!");
			}
		}

		private bool isPlayAnimation(int tagHash)
		{
			AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo (0);
			return stateInfo.tagHash == tagHash;
		}
	}
}
