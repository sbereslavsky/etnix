using System;
using System.Collections;
using UnityEngine;
using strange.extensions.mediation.impl;

using DigitalRuby.Tween;

namespace strangeetnix.game
{
	public class DropCoinMediator : Mediator
	{
		[Inject]
		public DropCoinView view { get; set; }

		[Inject]
		public IGameModel gameModel { get; set; }

		private Vector2Tween _tween = null;

		public override void OnRegister ()
		{
			view.init ();
			initListeners (true);
		}

		public override void OnRemove ()
		{
			initListeners (false);
		}

		private void initListeners (bool value)
		{
			if (value) {
				view.endDelaySignal.AddListener (onEndDelay);
				view.forceDestroySignal.AddListener (onForceDestroy);
			} else {
				view.endDelaySignal.RemoveListener (onEndDelay);
				view.forceDestroySignal.RemoveListener (onForceDestroy);
			}
		}

		private void onEndDelay()
		{
			Vector3 pos = view.gameObject.transform.position;

			GameObject player = GameObject.FindWithTag (PlayerView.ID);
			if (player != null) {
				Vector3 playerPos = player.transform.position;

				_tween = TweenFactory.Tween ("MoveCoin", new Vector2(pos.x, pos.y), new Vector2(playerPos.x, playerPos.y + 0.5f), 1f, TweenScaleFunctions.CubicEaseIn, (tweenProgress) => {
					view.gameObject.transform.position = new Vector3(tweenProgress.CurrentValue.x, tweenProgress.CurrentValue.y);
				}, (tweenComplete) => {
					onCompleteTween ();
				});
			} else {
				onCompleteTween ();
			}
		}

		private void onCompleteTween()
		{
			gameModel.coins += view.coinValue;
			Destroy (view.gameObject);
		}

		private void onForceDestroy()
		{
			if (_tween != null) {
				_tween.Stop (TweenStopBehavior.Complete);
				_tween = null;
			}
		}
	}
}

