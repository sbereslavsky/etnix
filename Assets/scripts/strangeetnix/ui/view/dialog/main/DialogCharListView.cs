using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;

namespace strangeetnix.ui
{
	public class DialogCharListView : EventView
	{
		internal const string ADD = "ADD";

		public Dropdown dropDown;
		public Button buttonAdd;

		internal Signal<int, int> addCharSignal = new Signal<int, int>();

		private int _dialogId;

		internal void init(List<string> charNames, int dialogId)
		{
			_dialogId = dialogId;

			buttonAdd.onClick.AddListener (addItem);

			if (dropDown != null) {
				dropDown.options.Clear ();
				if (charNames != null && charNames.Count > 0) {
					foreach (string c in charNames) {
						dropDown.options.Add (new Dropdown.OptionData () { text = c });
					}

					int TempInt = dropDown.value;
					dropDown.value = dropDown.value+1;
					dropDown.value = TempInt;
				} else {
					Destroy (dropDown);
				}
			}
		}

		private void addItem()
		{
			addCharSignal.Dispatch (_dialogId, dropDown.value + 1);
		}
	}
}
