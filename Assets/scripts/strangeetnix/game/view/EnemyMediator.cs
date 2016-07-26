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

		private bool _canToHit = false;

		private EnemyTriggerManager _enemyManager;
		private string _viewKey;

		public override void OnRegister ()
		{
			_enemyModel = gameModel.levelModel.getEnemyModelById (gameModel.createEnemyId);
			_startHp = _enemyModel.hp;
			_hp = _startHp;
			_damage = _enemyModel.damage;
			_cooldown = _enemyModel.cooldown;
			_expGive = _enemyModel.exp_give;
			_enemyManager = gameModel.levelModel.enemyManager;
			_viewKey = view.gameObject.name;

			view.init ();
			view.moveSpeed = _enemyModel.speed;
			view.updateHp (_hp, _startHp);

			//_enemyManager.setState (_viewKey, CharacterStates.MOVE);

			UpdateListeners (true);
		}

		public override void OnRemove ()
		{
			_enemyModel = null;
			StopAllCoroutines ();

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

				moveEnemySignal.AddListener (onMoveEnemy);
			} else {
				view.triggerEnterSignal.RemoveListener (onTriggerEnter);
				view.forceExitTriggerSignal.RemoveListener (onForceExitTrigger);
				view.triggerExitSignal.RemoveListener (onTriggerExit);

				view.hitPlayerSignal.RemoveListener (onStartHitPlayer);
				view.hitEnemySignal.RemoveListener (onHitByPlayer);

				moveEnemySignal.RemoveListener (onMoveEnemy);
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
					//addExpSignal.Dispatch (_expGive);
					gameModel.playerModel.addExp(_expGive);
					updateHudItemSignal.Dispatch (UpdateHudItemType.EXP, gameModel.playerModel.exp);

					_isDeath = true;
					_enemyManager.setState (_viewKey, CharacterStates.DEATH);
					destroyEnemySignal.Dispatch (view, _enemyModel.assetVO.delayToDestroy, false);
				}
			}
		}

		private void onMoveEnemy(bool canMove)
		{
			view.setCanMove (canMove);
			if (canMove) {
				_enemyManager.setState (_viewKey, CharacterStates.MOVE);
			} else {
				_enemyManager.setState (_viewKey, CharacterStates.IDLE);
			}
		}

		private void onStartHitPlayer()
		{
			if (view.canHit) {
				StopAllCoroutines ();
				view.canHit = false;
				_canToHit = true;
				StartCoroutine (hitPlayer ());
			} else {
				_enemyManager.setState (_viewKey, CharacterStates.MOVE);
			}
		}

		private IEnumerator hitPlayer()
		{
			yield return new WaitForSeconds (_enemyModel.assetVO.delayToHit);
			if (_canToHit) {
				_canToHit = false;
				if (_enemyModel.assetVO.hasExplosion) {
					addExplosionSignal.Dispatch (view.explosionPos);
				}
				hitPlayerSignal.Dispatch (transform, _damage);
			}
			StopAllCoroutines ();
			StartCoroutine (setCanHit());
		}

		private IEnumerator setCanHit()
		{
			yield return new WaitForSeconds (_cooldown);
			_canToHit = true;
			view.canHit = true;

			if (_enemyManager.checkBeforeHit (_viewKey)) {
				_enemyManager.setState (_viewKey, CharacterStates.HIT);
			} else {
				onForceExitTrigger (false);
			}
		}

		private void onForceExitTrigger(bool isPlayer)
		{
			/*if (_canToHit) {
				StopAllCoroutines ();
			}
			view.canHit = true;*/
			if (_canToHit) {
				_canToHit = false;
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