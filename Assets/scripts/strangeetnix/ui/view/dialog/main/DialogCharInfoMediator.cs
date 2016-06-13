using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using strangeetnix.game;

namespace strangeetnix.ui
{
	public class DialogCharInfoMediator : Mediator
	{
		[Inject]
		public DialogCharInfoView view{ get; set;}

		[Inject]
		public EditCharDataSignal editCharDataSignal{ get; set; }

		[Inject]
		public RemoveCharPanelSignal removeCharPanelSignal{ get; set; }

		[Inject]
		public ChoosePlayerSignal choosePlayerSignal{ get; set; }

		private string _viewName;

		public override void OnRegister()
		{
			_viewName = view.name;
			UpdateListeners(true);

			Debug.Log ("Add: "+_viewName);
		}

		public override void OnRemove()
		{
			UpdateListeners(false);
			Debug.Log ("Remove: "+_viewName);
		}

		private void UpdateListeners(bool value)
		{
			if (value) {
				view.editCharDataSignal.AddListener (onEditItem);
				view.removeCharPanelSignal.AddListener (onRemoveItem);
				view.startGameSceneSignal.AddListener (onStartGame);
			} else {
				view.editCharDataSignal.RemoveListener (onEditItem);
				view.removeCharPanelSignal.RemoveListener (onRemoveItem);
				view.startGameSceneSignal.RemoveListener (onStartGame);
			}
		}

		private void onEditItem(int panelId)
		{
			editCharDataSignal.Dispatch (panelId);
		}

		private void onRemoveItem(int panelId)
		{
			removeCharPanelSignal.Dispatch (panelId);
		}

		private void onStartGame(int charId)
		{
			choosePlayerSignal.Dispatch (charId);
		}
	}
}
