//Mediators provide a buffer between Views and the rest of the app.
//THIS IS A REALLY GOOD THING. READ ABOUT IT HERE:
//http://thirdmotion.github.io/strangeioc/faq.html#why-mediator

//This mediates between the app and the ShipView.

using System;
using System.Collections;
using System.Collections.Generic;
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
		private int _playerHitCount = 0;

		private List<GameObject> _enemyList;

		private bool _battleMode;

		private PlayerTriggerManager _playerTriggerManager;

		//This is the first (important) thing to happen in the Mediator. It tells
		//you that your mediator has been attached, so it's like Start() or a
		//Constructor. Do all your startup stuff here
		public override void OnRegister ()
		{
			_playerHitCount = 0;

			gameInputSignal.AddListener (onGameInput);

			_battleMode = gameModel.levelModel.hasEnemy;
			if (_battleMode) {
				_damage = gameModel.playerModel.damage;
				_cooldown = gameModel.playerModel.cooldown;

				_playerTriggerManager = gameModel.levelModel.playerTriggerManager;
				_playerTriggerManager.playerView = view;

				updateListeners (true);
			}

			view.init (_battleMode);
		}

		//OnRemove() is like a destructor/OnDestroy. Use it to clean up.
		public override void OnRemove ()
		{
			gameInputSignal.RemoveListener (onGameInput);

			if (_battleMode) {
				updateListeners (false);

				hitPlayerSignal.RemoveListener (onHitPlayer);
				view.hitEnemySignal.RemoveListener (onHitEnemy);
			}

			StopAllCoroutines ();
			//gameInputSignal.RemoveListener (onGameInput);
		}

		private void updateListeners(bool value)
		{
			if (value) {
				view.triggerEnterSignal.AddListener (onTriggerEnter);
				view.triggerExitSignal.AddListener (onTriggerExit);

				view.hitEnemySignal.AddListener (onHitEnemy);

				hitPlayerSignal.AddListener (onHitPlayer);
			} else {
				view.triggerEnterSignal.RemoveListener (onTriggerEnter);
				view.triggerExitSignal.RemoveListener (onTriggerExit);

				view.hitEnemySignal.RemoveListener (onHitEnemy);

				hitPlayerSignal.RemoveListener (onHitPlayer);
			}	
		}

		private void onTriggerEnter(Collider2D enemyCollider)
		{
			_playerTriggerManager.addTrigger (enemyCollider);
		}

		private void onTriggerExit(string enemyName)
		{
			_playerTriggerManager.removeTriggerByKey (enemyName);
		}

		private void onHitEnemy()
		{
			List<GameObject> enemyList = _playerTriggerManager.getEnemyToHit ();

			if (view.canHit) {
				view.canHit = false;
				_enemyList = enemyList;
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
			if (_enemyList != null) {
				foreach (GameObject enemyGO in _enemyList) {
					EnemyView enemyView = enemyGO.GetComponent<EnemyView> ();
					if (enemyView != null) {
						updateHudItemSignal.Dispatch (UpdateHudItemType.COOLDOWN, _cooldown);
						enemyView.hitEnemySignal.Dispatch (_damage);
					}
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
				gameModel.playerModel.resetHp ();
			}

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
			//SetAction (input);

			/*if ((input & GameInputEvent.FIRE) > 0)
			{
				//fireMissileSignal.Dispatch (gameObject, GameElement.MISSILE_POOL);
			}*/
		}

		//keyboard event!
		//Set the IME value
		internal void SetAction(int evt)
		{
			int input = evt;
			bool left = (input & GameInputEvent.LEFT) > 0;
			bool right = (input & GameInputEvent.RIGHT) > 0;
			bool hit = (input & GameInputEvent.HIT) > 0;
			if (input == 0) {
				view.stopWalk ();
			}

			if (left) {
				view.startLeftWalk ();
			}

			if (right) {
				view.startRightWalk ();
			}

			if (hit) {
				_playerHitCount++;
				if (_playerHitCount % 2 == 0) {
					view.startHit2 ();
				} else {
					view.startHit1 ();
				}
			}
		}
	}
}

