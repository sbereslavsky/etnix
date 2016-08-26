﻿//Mediators provide a buffer between Views and the rest of the app.
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
		public StopEnemySignal stopEnemySignal{ get; set; }

		[Inject]
		public HitPlayerSignal hitPlayerSignal{ get; set; }

		[Inject]
		public UpdateHudItemSignal updateHudItemSignal{ get; set; }

		[Inject]
		public AddExplosionSignal addExplosionSignal{ get; set; }

		[Inject]
		public CreateCoinSignal createCoinSignal{ get; set; }

		//[Inject]
		//public AddExpSignal addExpSignal{ get; set; }

		//Model
		[Inject]
		public IGameModel gameModel{ get; set; }

		private int _hp;
		private int _damage;
		private float _cooldown;
		private int _expGive;

		private int _goldDrop;

		private bool _isDeath = false;

		private IEnemyModel _enemyModel;

		private int _startHp = 0;

		private bool _canToHit = false;

		private EnemyTriggerManager _enemyManager;
		private string _viewKey;

		private bool _canAction = true;

		private IEnumerator _hitPlayer;
		private IEnumerator _setCanHit;

		public override void OnRegister ()
		{
			_enemyModel = gameModel.roomModel.getEnemyModelById (gameModel.createEnemyId);
			_startHp = _enemyModel.hp;
			_hp = _startHp;
			_damage = _enemyModel.damage;
			_cooldown = _enemyModel.cooldown;
			_expGive = _enemyModel.exp_give;
			_enemyManager = gameModel.roomModel.enemyManager;
			_viewKey = view.gameObject.name;

			_goldDrop = UnityEngine.Random.Range (_enemyModel.gold_drop_min, _enemyModel.gold_drop_max);

			view.init ();
			view.moveSpeed = _enemyModel.speed;
			view.updateHp (_hp, _startHp);

			//_enemyManager.setState (_viewKey, CharacterStates.MOVE);

			UpdateListeners (true);
		}

		public override void OnRemove ()
		{
			_enemyModel = null;
			stopMediatorCorutines ();

			UpdateListeners (false);
		}

		private void UpdateListeners(bool value)
		{
			if (value) {
				view.triggerEnterSignal.AddListener (onTriggerEnter);
				view.forceExitTriggerSignal.AddListener (onForceExitTrigger);
				view.triggerExitSignal.AddListener (onTriggerExit);

				view.hitPlayerSignal.AddListener (onStartHitPlayer);
				view.hitEnemySignal.AddListener (onHitByPlayer);

				stopEnemySignal.AddListener (onStopEnemy);
			} else {
				view.triggerEnterSignal.RemoveListener (onTriggerEnter);
				view.forceExitTriggerSignal.RemoveListener (onForceExitTrigger);
				view.triggerExitSignal.RemoveListener (onTriggerExit);

				view.hitPlayerSignal.RemoveListener (onStartHitPlayer);
				view.hitEnemySignal.RemoveListener (onHitByPlayer);

				stopEnemySignal.RemoveListener (onStopEnemy);
			}	
		}

		private void onHitByPlayer(int decHp)
		{
			if (!_isDeath) {
				_hp = Mathf.Max (0, _hp - decHp);
				view.updateHp (_hp, _startHp);
				// If the enemy has zero or fewer hit points and isn't dead yet...
				if (_hp > 0) {
					if (view.canToDefeat ()) {
						_enemyManager.setState (_viewKey, CharacterStates.DEFEAT);
					}
				} else  {
					StopAllCoroutines ();

					Vector2 position = new Vector2 (view.gameObject.transform.position.x, view.gameObject.transform.position.y);
					createCoinSignal.Dispatch (position, _goldDrop);

					gameModel.playerModel.addExp(_expGive);
					updateHudItemSignal.Dispatch (UpdateHudItemType.EXP, gameModel.playerModel.exp);

					_isDeath = true;
					_enemyManager.setState (_viewKey, CharacterStates.DEATH);
					destroyEnemySignal.Dispatch (view, _enemyModel.assetVO.delayToDestroy, false);
				}
			}
		}

		private void onStopEnemy()
		{
			_canAction = false;
			view.disableCanMove ();
			stopMediatorCorutines ();
			//_enemyManager.setState (_viewKey, CharacterStates.IDLE);
		}

		private void onStartHitPlayer()
		{
			if (_canAction) {
				if (view.canHit) {
					StopAllCoroutines ();
					view.canHit = false;
					_canToHit = true;
					_hitPlayer = hitPlayer ();
					StartCoroutine (_hitPlayer);
				} else {
					_enemyManager.setState (_viewKey, CharacterStates.MOVE);
				}
			}
		}

		private IEnumerator hitPlayer()
		{
			yield return new WaitForSeconds (_enemyModel.assetVO.delayToHit);
			if (_hitPlayer != null) {
				StopCoroutine (_hitPlayer);
				_hitPlayer = null;
			}
			Debug.Log ("EnemyMediator.hitPlayer. id = " + view.name + ". _canAction = " + _canAction.ToString());
			if (_canAction) {
				if (_canToHit) {
					StopAllCoroutines ();
					// check to hit player, other exit trigger
					if (_enemyManager.checkBeforeHit (_viewKey)) {
						_canToHit = false;
						if (_enemyModel.assetVO.hasExplosion) {
							addExplosionSignal.Dispatch (view.explosionPos);
						}
						hitPlayerSignal.Dispatch (transform, _damage);

						_setCanHit = setCanHit ();
						StartCoroutine (_setCanHit);
					} else {
						_canToHit = true;
						view.canHit = true;
						onForceExitTrigger (false);
					}
				}
			}
		}

		private IEnumerator setCanHit()
		{
			yield return new WaitForSeconds (_cooldown);
			if (_setCanHit != null) {
				StopCoroutine (_setCanHit);
				_setCanHit = null;
			}

			Debug.Log ("EnemyMediator.setCanHit. id = " + view.name + ". _canAction = " + _canAction.ToString());
			if (_canAction) {
				_canToHit = true;
				view.canHit = true;

				if (_enemyManager.checkBeforeHit (_viewKey)) {
					_enemyManager.setState (_viewKey, CharacterStates.HIT);
				} else {
					onForceExitTrigger (false);
				}
			}
		}

		private void stopMediatorCorutines()
		{
			if (_hitPlayer != null) {
				StopCoroutine (_hitPlayer);
				_hitPlayer = null;
			}

			if (_setCanHit != null) {
				StopCoroutine (_setCanHit);
				_setCanHit = null;
			}
		}

		private void onForceExitTrigger(bool isPlayer)
		{
			if (_canToHit) {
				_canToHit = false;
			}

			if (!view.canHit && _setCanHit == null) {
				view.canHit = true;
			}

			if (isPlayer) {
				_enemyManager.forceExitPlayerTrigger (_viewKey);
			}
		}

		private void onTriggerEnter(GameObject enemyGO = null)
		{
			string otherName = (enemyGO != null) ? enemyGO.name : null;
			_enemyManager.addTrigger (_viewKey, otherName);
		}

		private void onTriggerExit(GameObject enemyGO)
		{
			_enemyManager.exitTrigger (_viewKey, enemyGO);
		}
	}
}