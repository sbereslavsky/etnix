using System;
using System.Collections;
using UnityEngine;
using strange.extensions.mediation.impl;

namespace strangeetnix.game
{
	public class CharacterView : View
	{
		protected Animator _anim; 
		protected bool _isDead;					// Whether or not the enemy is dead.
		//protected Transform _frontCheck;		// Reference to the position of the gameobject used for checking if something is in front.
		protected Transform _explosion;

		protected Rigidbody2D _rigidBody;
		protected BoxCollider2D _collider2d;

		protected bool _isWait = false;
		protected float _waitTime = 0f;

		virtual public void init () 
		{ 
			init2 ();
		} 
		virtual public void init (bool battleMode) 
		{
			init2 ();
		}

		private void init2()
		{
			_isDead = false;
			_anim = GetComponent<Animator>();

			_rigidBody = GetComponent<Rigidbody2D> ();
			_collider2d = GetComponent<BoxCollider2D> ();
		}

		protected bool isCollisionOut(Collider2D other)
		{
			float dist1 = other.bounds.SqrDistance (_collider2d.bounds.center);
			float width = (other as BoxCollider2D).size.x + _collider2d.size.x;
			bool result = (width * 0.3f > dist1 || dist1 > width * 1.2f);
			return result;
		}

		virtual public void setDeath () 
		{
			this.enabled = false;
		}

		public void destroyView(float time)
		{
			Destroy (this.gameObject, time);
		}

		protected void startWait(IEnumerator routine, float time)
		{
			if (!_isWait) {
				_waitTime = time;
				_isWait = true;
				StartCoroutine (routine);
			}
		}

		virtual protected bool isPlayAnimation (string type)
		{
			return _anim.GetBool(type);
		}

		virtual protected void flip()
		{
			// Multiply the x component of localScale by -1.
			Vector3 transformScale = transform.localScale;
			transformScale.x *= -1;
			transform.localScale = transformScale;
		}

		protected void playAnimation (string animationId)
		{
			if (_anim) {
				_anim.SetTrigger (animationId);
			} else {
				Debug.Log ("playAnimation(" + animationId + "). anim = null!");
			}
		}

		protected bool isEqualsNames(GameObject go)
		{
			return gameObject.name.Equals (go.name);
		}

		protected bool isEqualsScaleX(GameObject go)
		{
			return gameObject.transform.localScale.x == go.transform.localScale.x;
		}

		protected bool isPlayerObject(string tag)
		{
			return tag.Contains(PlayerView.ID);
		}

		protected bool isOtherEnemy(Collider2D other)
		{
			return (other.tag.Contains(EnemyView.ID) && !isEqualsNames(other.gameObject));
		}
	}
}

