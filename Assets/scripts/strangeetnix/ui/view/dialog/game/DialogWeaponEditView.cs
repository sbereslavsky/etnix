using System;
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
	public class DialogWeaponEditView : TransformDialogView
	{
		public Text textGoldValue;
		public Text textWeaponInfo;

		public Dropdown dropDownEquiped;
		public Dropdown dropDownWeapon;
		public Dropdown dropDownItem2;
		public Dropdown dropDownItem3;

		public Button buttonClose;

		internal Signal<int> changeWeaponSignal = new Signal<int>();
		internal Signal<int> changeItem2Signal = new Signal<int>();
		internal Signal<int> changeItem3Signal = new Signal<int>();
		internal Signal<int> changeEquipedSignal = new Signal<int>();
		internal Signal closeDialogSignal = new Signal();

		public int charId { get; set; }

		private IGameConfig _gameConfig;

		internal void init(IUserCharVO userCharVO, IGameConfig gameConfig)
		{
			_gameConfig = gameConfig;

			List<string> info_names = gameConfig.weaponConfig.getInfoListByOwnerId (userCharVO.classId);

			int weaponId = (info_names.Count < userCharVO.weaponId) ? userCharVO.weaponId / info_names.Count : userCharVO.weaponId;
			initDropDown (dropDownWeapon, info_names, weaponId-1);
			initDropDown (dropDownItem2, gameConfig.itemConfig.info_names, userCharVO.itemId2-1);
			initDropDown (dropDownItem3, gameConfig.itemConfig.info_names, userCharVO.itemId3-1);
			initDropDown (dropDownEquiped, gameConfig.equipedConfig.info_names, userCharVO.equipedId-1);

			addEventListeners ();
		}

		private void addEventListeners()
		{
			dropDownWeapon.onValueChanged.AddListener(delegate {
				dropDownWeaponChangedHandler(dropDownWeapon);
			});

			dropDownItem2.onValueChanged.AddListener(delegate {
				dropDownItem2ChangedHandler(dropDownItem2);
			});

			dropDownItem3.onValueChanged.AddListener(delegate {
				dropDownItem3ChangedHandler(dropDownItem3);
			});

			dropDownEquiped.onValueChanged.AddListener(delegate {
				dropDownEquipedChangedHandler(dropDownEquiped);
			});

			buttonClose.onClick.AddListener (saveAndExit);
		}

		public void removeEventListeners()
		{
			dropDownWeapon.onValueChanged.RemoveListener(delegate {
				dropDownWeaponChangedHandler(dropDownWeapon);
			});

			dropDownItem2.onValueChanged.RemoveListener(delegate {
				dropDownItem2ChangedHandler(dropDownItem2);
			});

			dropDownItem3.onValueChanged.RemoveListener(delegate {
				dropDownItem3ChangedHandler(dropDownItem3);
			});

			dropDownEquiped.onValueChanged.RemoveListener(delegate {
				dropDownEquipedChangedHandler(dropDownEquiped);
			});

			buttonClose.onClick.RemoveListener (saveAndExit);
		}

		private void dropDownWeaponChangedHandler(Dropdown target) 
		{
			string value = getDropDownText (target);
			int weaponId = _gameConfig.weaponConfig.getIdByInfo (value);
			changeWeaponSignal.Dispatch (weaponId);
		}

		private void dropDownItem2ChangedHandler(Dropdown target) 
		{
			string value = getDropDownText (target);
			int item2Id = _gameConfig.itemConfig.getIdByInfo (value);
			changeItem2Signal.Dispatch (item2Id);
		}

		private void dropDownItem3ChangedHandler(Dropdown target) 
		{
			string value = getDropDownText (target);
			int item3Id = _gameConfig.itemConfig.getIdByInfo (value);
			changeItem3Signal.Dispatch (item3Id);
		}

		private void dropDownEquipedChangedHandler(Dropdown target) 
		{
			string value = getDropDownText (target);
			int equipedId = _gameConfig.equipedConfig.getIdByInfo (value);
			changeEquipedSignal.Dispatch (equipedId);
		}

		public void updateGoldValue(int value)
		{
			textGoldValue.text = value.ToString ();
		}

		public void updatePlayerInfo(IPlayerModel playerModel)
		{
			textWeaponInfo.text = "Weapon dmg = " + playerModel.weaponVO.damage + ", cldwn = " + playerModel.weaponVO.cooldown + ". player dmg = " + playerModel.damage + ", cldwn = " + playerModel.cooldown;
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
			closeDialogSignal.Dispatch ();
		}

		public void clearDropDowns()
		{
			dropDownEquiped.options.Clear ();
			dropDownWeapon.options.Clear ();
			dropDownItem2.options.Clear ();
			dropDownItem3.options.Clear ();
		}

		private string getDropDownText(Dropdown dropDown)
		{
			int selectId = dropDown.value;
			string result = dropDown.options [selectId].text;
			return result;
		}
	}
}

