//Mediators provide a buffer between Views and the rest of the app.
//THIS IS A REALLY GOOD THING. READ ABOUT IT HERE:
//http://thirdmotion.github.io/strangeioc/faq.html#why-mediator

//This mediates between the app and the ShipView.

using System;
using System.Collections;
using UnityEngine;
using strange.extensions.mediation.impl;

namespace strangeetnix.game
{
	public class PlayerMediator : Mediator
	{
		//View
		[Inject]
		public PlayerView view { get; set; }

		//Signals
		[Inject]
		public GameInputSignal gameInputSignal{ get; set; }

		[Inject]
		public UpdateHudItemSignal updateHudItemSignal{ get; set; }

		[Inject]
		public HitPlayerSignal hitPlayerSignal{ get; set; }

		[Inject]
		public HitEnemySignal hitEnemySignal{ get; set; }

		[Inject]
		public DestroyPlayerSignal destroyPlayerSignal{ get; set; }

		[Inject]
		public AddExplosionSignal addExplosionSignal{ get; set; }

		[Inject]
		public AddHpSignal addHpSignal{ get; set; }

		//Model
		[Inject]
		public IGameModel gameModel{ get; set; }

		private int _damage = 0;
		private int _cooldown = 0;
		private GameObject _enemyGO;

		private bool _battleMode;

		//This is the first (important) thing to happen in the Mediator. It tells
		//you that your mediator has been attached, so it's like Start() or a
		//Constructor. Do all your startup stuff here
		public override void OnRegister ()
		{
			gameInputSignal.AddListener (onGameInput);

			_battleMode = gameModel.levelModel.hasEnemy;
			if (_battleMode) {
				_damage = gameModel.playerModel.damage;
				_cooldown = gameModel.playerModel.cooldown;

				hitPlayerSignal.AddListener (onHitPlayer);

				view.hitEnemySignal.AddListener (onHitEnemy);
			}

			view.init (_battleMode);

			//view.collisionSignal.AddListener (onCollision);
		}

		//OnRemove() is like a destructor/OnDestroy. Use it to clean up.
		public override void OnRemove ()
		{
			gameInputSignal.RemoveListener (onGameInput);

			if (_battleMode) {
				hitPlayerSignal.RemoveListener (onHitPlayer);
				view.hitEnemySignal.RemoveListener (onHitEnemy);
			}

			StopAllCoroutines ();
			//view.collisionSignal.RemoveListener (onCollision);
			//gameInputSignal.RemoveListener (onGameInput);
		}

		private void onHitEnemy(GameObject enemyGO)
		{
			if (view.canHit) {
				view.canHit = false;
				_enemyGO = enemyGO;
				StartCoroutine (pauseBeforeHit());
			}
		}

		private IEnumerator pauseBeforeHit()
		{
			yield return new WaitForSeconds (gameModel.playerModel.assetVO.delayToHit);
			hitEnemy ();
			if (gameModel.playerModel.assetVO.hasExplosion) {
				addExplosionSignal.Dispatch (view.explosionPos);
			}
			/*if (_cooldown > 0) {
				StartCoroutine (startCooldown());
			} else {
				view.canHit = true;
			}*/
			view.canHit = true;
			StopCoroutine (pauseBeforeHit());
		}

		private IEnumerator startCooldown()
		{
			yield return new WaitForSeconds (_cooldown);
			view.canHit = true;
			StopCoroutine (startCooldown());
		}

		private void hitEnemy()
		{
			if (_enemyGO) {
				EnemyView enemyView = _enemyGO.GetComponent<EnemyView> ();
				if (enemyView != null) {
					updateHudItemSignal.Dispatch (UpdateHudItemType.COOLDOWN, _cooldown);
					hitEnemySignal.Dispatch (enemyView, _damage);
				}
			}
		}

		private void onHitPlayer(Transform enemyTransform, int decHp)
		{
			//check to turn player to enemy
			view.checkToFlip(enemyTransform);

			addHpSignal.Dispatch (decHp, false);

			//gameModel.playerModel.hp = Mathf.Max (0, gameModel.playerModel.hp - decHp);
			//updatePlayerHpSignal.Dispatch (gameModel.playerModel.hp);

			if (gameModel.playerModel.hp == 0) {
				view.setDeath ();
				destroyPlayerSignal.Dispatch (view, gameModel.playerModel.assetVO.delayToDestroy, false);
			} else {
				view.playDefeatAnimation ();
			}
		}

		//Receive a signal updating GameInput
		private void onGameInput(int input)
		{
			//view.SetAction (input);

			/*if ((input & GameInputEvent.FIRE) > 0)
			{
				//fireMissileSignal.Dispatch (gameObject, GameElement.MISSILE_POOL);
			}*/
		}
	}
}

