using System;
using UnityEngine;
using strange.extensions.mediation.impl;

namespace strangeetnix.game
{
	public class CharacterView : View
	{
		protected Animator _anim; 
		protected bool _isDead;					// Whether or not the enemy is dead.
		protected Transform _frontCheck;			// Reference to the position of the gameobject used for checking if something is in front.
		protected Transform _explosion;

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
		}

		virtual public void setDeath () 
		{
			this.enabled = false;
		}

		public void destroyView(float time)
		{
			Destroy (this.gameObject, time);
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
	}
}

