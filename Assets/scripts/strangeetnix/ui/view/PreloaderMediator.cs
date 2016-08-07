using System;
using UnityEngine;
using strange.extensions.mediation.impl;
using strangeetnix.game;

namespace strangeetnix.ui
{
	public class PreloaderMediator : Mediator
	{
		[Inject]
		public PreloaderView view{ get; set; }

		[Inject]
		public DestroyPreloaderSignal destroyPreloaderSignal{ get; set; }

		[Inject]
		public UpdatePreloaderValueSignal updatePreloaderValueSignal{ get; set; }

		public override void OnRegister ()
		{
			onUpdatePreloaderValue (0);
			UpdateListeners (true);
		}

		private void UpdateListeners(bool value)
		{
			if (value) {
				updatePreloaderValueSignal.AddListener (onUpdatePreloaderValue);
				destroyPreloaderSignal.AddListener (onDestroyView);
			} else {
				updatePreloaderValueSignal.RemoveListener (onUpdatePreloaderValue);
				destroyPreloaderSignal.RemoveListener (onDestroyView);
			}
		}

		private void onDestroyView()
		{
			Destroy (view.gameObject);
		}

		private void onUpdatePreloaderValue(int value)
		{
			view.setText (value);
		}

		public override void OnRemove ()
		{
			UpdateListeners (false);
		}
	}
}

