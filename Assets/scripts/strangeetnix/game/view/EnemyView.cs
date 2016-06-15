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

		internal Signal collisionSignal = new Signal ();

		public float moveSpeed = 2f;			// The speed the enemy moves at.

		private Vector3 _hpScale;
		private Transform _hpTransform;
		private Transform _hpBgTransform;

		private bool _isMove = false;

		private bool _canMove = true;

		internal bool canHit = true;

		public override void init()
		{
			// Setting up the references.
			_frontCheck = transform.Find(FRONT_CHECK).transform;
			_explosion = transform.Find(EXPLOSION).transform;
			_hpTransform = transform.Find (HEALTH).transform;
			_hpBgTransform = transform.Find (HEALTH_BG).transform;
			_hpScale = _hpTransform.localScale;

			base.init ();

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
			
		internal void SetCanMove(bool value) 
		{
			_canMove = value;
			/*if (_canMove) {
				checkToFlip ();
			}*/
		}

		void FixedUpdate ()
		{
			if (_canMove && !isPlayAnimation (EnemyAnimatorTypes.TRIGGER_DEFEAT)) {
				// Create an array of all the colliders in front of the enemy.
				Collider2D[] frontHits = Physics2D.OverlapPointAll(_frontCheck.position);

				// Check each of the colliders.
				foreach(Collider2D c in frontHits)
				{
					bool isPlayer = c.tag == PlayerView.ID;
					bool isEnemy = c.tag == ID && !this.gameObject.Equals(c.gameObject);
					// If any of the colliders is an Obstacle...
					if (isPlayer || isEnemy) {
						if (isPlayer && c.gameObject != null && canHit) {
							postHitPlayer ();
						} else if (isEnemy) {
							if (!isPlayAnimation(EnemyAnimatorTypes.TRIGGER_IDLE)) {
								playAnimation (EnemyAnimatorTypes.TRIGGER_IDLE);
							}
						}
						_isMove = false;
						return;
					}
				}

				if (!_isMove) {
					_isMove = true;
					playAnimation (EnemyAnimatorTypes.TRIGGER_WALK);
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

		private void postHitPlayer()
		{
			playAnimation (EnemyAnimatorTypes.TRIGGER_HIT);
			collisionSignal.Dispatch();
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
			if (_anim && !isPlayAnimation(EnemyAnimatorTypes.TRIGGER_DEFEAT) && !isPlayAnimation(EnemyAnimatorTypes.TRIGGER_HIT)) {
				_isMove = false; 
				playAnimation (EnemyAnimatorTypes.TRIGGER_DEFEAT);
			}
		}

		override public void setDeath()
		{
			base.setDeath ();
			playAnimation (EnemyAnimatorTypes.TRIGGER_DEATH);
			Rigidbody2D rigidbody2D = this.GetComponent<Rigidbody2D> ();
			if (rigidbody2D != null) {
				rigidbody2D.isKinematic = true;
			}

			BoxCollider2D boxCollider2D = this.GetComponent<BoxCollider2D> ();
			if (boxCollider2D != null) {
				boxCollider2D.enabled = false;
			}
		}
	}
}
