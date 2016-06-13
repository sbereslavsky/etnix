//Mediators provide a buffer between Views and the rest of the app.
//THIS IS A REALLY GOOD THING. READ ABOUT IT HERE:
//http://thirdmotion.github.io/strangeioc/faq.html#why-mediator

//This mediates between the app and the EnemyView.

using System;
using System.Collections;
using UnityEngine;
using strange.extensions.mediation.impl;

namespace strangeetnix.game
{
	public class EnemyMediator : Mediator
	{
		[Inject]
		public EnemyView view { get; set; }

		//Signals
		[Inject]
		public DestroyEnemySignal destroyEnemySignal{ get; set; }

		[Inject]
		public MoveEnemySignal moveEnemySignal{ get; set; }

		[Inject]
		public HitEnemySignal hitEnemySignal{ get; set; }

		[Inject]
		public HitPlayerSignal hitPlayerSignal{ get; set; }

		[Inject]
		public UpdateHudItemSignal updateHudItemSignal{ get; set; }

		[Inject]
		public AddExplosionSignal addExplosionSignal{ get; set; }

		//[Inject]
		//public AddExpSignal addExpSignal{ get; set; }

		//Model
		[Inject]
		public IGameModel gameModel{ get; set; }

		private int _hp;
		private int _damage;
		private float _cooldown;
		private int _expGive;

		private bool _isDeath = false;

		private IEnemyModel _enemyModel;

		private int _startHp = 0;

		public override void OnRegister ()
		{
			_enemyModel = gameModel.levelModel.getEnemyModelById (gameModel.createEnemyId);
			_startHp = _enemyModel.hp;
			_hp = _startHp;
			_damage = _enemyModel.damage;
			_cooldown = _enemyModel.cooldown;
			_expGive = _enemyModel.exp_give;

			view.init ();
			view.moveSpeed = _enemyModel.speed;
			view.collisionSignal.AddListener (onPlayerCollision);
			view.updateHp (_hp, _startHp);

			hitEnemySignal.AddListener (onHitByPlayer);
			moveEnemySignal.AddListener (onMoveEnemy);

			/*int enemyCount = gameModel.levelModel.enemyCount;
			Collider2D collider = view.GetComponent<Collider2D> ();
			if (collider != null) {
				collider.offset = new Vector2 (0, collider.offset.y*enemyCount);
			}*/
		}

		public override void OnRemove ()
		{
			_enemyModel = null;
			StopAllCoroutines ();
			hitEnemySignal.RemoveListener (onHitByPlayer);
			moveEnemySignal.RemoveListener (onMoveEnemy);

			view.collisionSignal.RemoveListener (onPlayerCollision);
		}

		private void onMoveEnemy(bool canMove)
		{
			view.SetCanMove (canMove);
		}

		private void onPlayerCollision()
		{
			if (view.canHit) {
				view.canHit = false;
				StartCoroutine (hitPlayer());
			}
		}

		private IEnumerator hitPlayer()
		{
			yield return new WaitForSeconds (_enemyModel.assetVO.delayToHit);
			if (_enemyModel.assetVO.hasExplosion) {
				addExplosionSignal.Dispatch (view.getExplosionPos ());
			}
			hitPlayerSignal.Dispatch (transform, _damage);
			StartCoroutine (setCanHit());
			StopCoroutine (hitPlayer());
		}

		private IEnumerator setCanHit()
		{
			yield return new WaitForSeconds (_cooldown);
			view.canHit = true;
			StopCoroutine (setCanHit());
		}

		private void onHitByPlayer(EnemyView enemyView, int decHp)
		{
			if (view.Equals(enemyView) && !_isDeath) {
				_hp = Mathf.Max (0, _hp - decHp);
				view.updateHp (_hp, _startHp);
				// If the enemy has zero or fewer hit points and isn't dead yet...
				if (_hp > 0) {
					view.hitByPlayer ();
				} else  {
					//addExpSignal.Dispatch (_expGive);
					gameModel.playerModel.addExp(_expGive);
					updateHudItemSignal.Dispatch (UpdateHudItemType.EXP, gameModel.playerModel.exp);

					_isDeath = true;
					StopAllCoroutines ();
					view.setDeath ();
					destroyEnemySignal.Dispatch (view, _enemyModel.assetVO.delayToDestroy, false);
				}
			}
		}
	}
}