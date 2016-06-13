using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;

namespace strangeetnix.ui
{
	public class DialogCharListMediator : Mediator
	{
		[Inject]
		public DialogCharListView view{ get; set;}

		[Inject]
		public AddCharPanelSignal addCharPanelSignal{ get; set; }

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
				view.addCharSignal.AddListener (onAdd);
			} else {
				view.addCharSignal.RemoveListener (onAdd);
			}
		}

		private void onAdd(int panelId, int charId)
		{
			addCharPanelSignal.Dispatch (panelId, charId);
		}
	}
}