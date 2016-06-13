﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;
using strangeetnix.game;

namespace strangeetnix.ui
{
	public class DialogCharEditView : EventView
	{
		private string TITLE_EXP = "Exp: ";

		public Text textName;
		public Text textLevel;
		public Text textExp;

		public Dropdown dropDownEquiped;
		public Dropdown dropDownWeapon;
		public Dropdown dropDownItem2;
		public Dropdown dropDownItem3;

		public InputField inputField;

		public Button buttonAddExp;
		public Button buttonSaveAndExit;
		public Button buttonExit;

		internal Signal<bool> closeDialogSignal = new Signal<bool>();
		internal Signal<int> addExpSignal = new Signal<int>();

		public int charId { get; set; }

		internal void init(IUserCharVO userCharVO, IGameConfig gameConfig)
		{
			List<string> info_names = gameConfig.weaponConfig.getInfoListByOwnerId (userCharVO.classId);
			int weaponId = (info_names.Count < userCharVO.weaponId) ? userCharVO.weaponId / info_names.Count : userCharVO.weaponId;
			initDropDown (dropDownWeapon, info_names, weaponId-1);
			initDropDown (dropDownItem2, gameConfig.itemConfig.info_names, userCharVO.itemId2-1);
			initDropDown (dropDownItem3, gameConfig.itemConfig.info_names, userCharVO.itemId3-1);
			initDropDown (dropDownEquiped, gameConfig.equipedConfig.info_names, userCharVO.equipedId-1);

			buttonAddExp.onClick.AddListener (addExp);
			buttonSaveAndExit.onClick.AddListener (saveAndExit);
			buttonExit.onClick.AddListener (exit);
		}

		public void updateInfo(IUserCharInfoVO userCharInfoVO)
		{
			textName.text = userCharInfoVO.name;
			textLevel.text = userCharInfoVO.level + " level. Hp: " + userCharInfoVO.hp;
			textExp.text = TITLE_EXP + userCharInfoVO.exp;
		}

		private void initDropDown(Dropdown dropDown, List<string> values, int selectable = 0)
		{
			if (dropDown != null && values != null && values.Count > 0) {
				dropDown.options.Clear ();

				if (values != null && values.Count > 0) {
					foreach (string c in values) {
						dropDown.options.Add (new Dropdown.OptionData () { text = c });
					}

					if (selectable == 0 || dropDown.options.Count < selectable) {
						int TempInt = dropDown.value;
						dropDown.value = dropDown.value + 1;
						dropDown.value = TempInt;
					} else {
						dropDown.value = selectable;
					}

				} else {
					Destroy (dropDown);
				}
			}
		}

		private void saveAndExit()
		{
			closeDialogSignal.Dispatch (true);
		}

		private void exit()
		{
			closeDialogSignal.Dispatch (false);
		}

		private void addExp()
		{
			int addValue = Convert.ToInt32 (inputField.text);
			if (addValue != 0) {
				addExpSignal.Dispatch (addValue);
			}
		}

		public void clearDropDowns()
		{
			dropDownEquiped.options.Clear ();
			dropDownWeapon.options.Clear ();
			dropDownItem2.options.Clear ();
			dropDownItem3.options.Clear ();
		}

		public string getDropDownText(Dropdown dropDown)
		{
			int selectId = dropDown.value;
			string result = dropDown.options [selectId].text;
			return result;
		}
	}
}