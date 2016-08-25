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
	public class DialogCharEditView : EventView
	{
		private string TITLE_EXP = "Exp: ";

		public Text textName;
		public Text textLevel;
		public Text textExp;
		public Text textPlayerInfo;

		public InputField inputField;

		public Button buttonAddExp;
		public Button buttonSaveAndExit;
		public Button buttonExit;

		internal Signal<bool> closeDialogSignal = new Signal<bool>();
		internal Signal<int> addExpSignal = new Signal<int>();

		public int charId { get; set; }

		private IUserCharVO _userCharVO;	
		private IGameConfig _gameConfig;
		private int _oldWeaponId;

		internal void init(IUserCharVO userCharVO, IGameConfig gameConfig)
		{
			_userCharVO = userCharVO;
			_gameConfig = gameConfig;
			_oldWeaponId = _userCharVO.weaponId;

			List<string> info_names = gameConfig.weaponConfig.getInfoListByOwnerId (userCharVO.classId);

			updatePlayerInfo ();

			buttonAddExp.onClick.AddListener (addExp);
			buttonSaveAndExit.onClick.AddListener (saveAndExit);
			buttonExit.onClick.AddListener (exit);
		}

		public void updatePlayerInfo()
		{
			IPlayerModel playerModel = new PlayerModel (_userCharVO.id, _gameConfig);
			textPlayerInfo.text = "Weapon dmg = " + playerModel.weaponVO.damage + ", cldwn = " + playerModel.weaponVO.cooldown + ". player dmg = " + playerModel.damage + ", cldwn = " + playerModel.cooldown;
		}

		public void updateInfo(IUserCharInfoVO userCharInfoVO)
		{
			textName.text = userCharInfoVO.name;
			textLevel.text = userCharInfoVO.level + " level. Hp: " + userCharInfoVO.hp;
			textExp.text = TITLE_EXP + userCharInfoVO.exp;
		}

		private void saveAndExit()
		{
			closeDialogSignal.Dispatch (true);
		}

		private void exit()
		{
			_userCharVO.weaponId = _oldWeaponId;
			closeDialogSignal.Dispatch (false);
		}

		private void addExp()
		{
			int addValue = Convert.ToInt32 (inputField.text);
			if (addValue != 0) {
				addExpSignal.Dispatch (addValue);
				updatePlayerInfo ();
			}
		}
	}
}