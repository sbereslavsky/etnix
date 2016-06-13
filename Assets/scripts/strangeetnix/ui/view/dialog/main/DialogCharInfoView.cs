using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;

namespace strangeetnix.ui
{
	public class DialogCharInfoView : View
	{
		private string TITLE_NAME = "Name: ";
		private string TITLE_HP = "Hp: ";
		private string TITLE_EXP = "Exp: ";
		private string TITLE_WEAPON = "Weapon: ";
		private string TITLE_ITEM1 = "Item1: ";
		private string TITLE_ITEM2 = "Item2: ";
		private string TITLE_EQUIPED = "Equped: ";

		public Text textName;
		public Text textHp;
		public Text textExp;
		public Text textWeapon;
		public Text textItem1;
		public Text textItem2;
		public Text textEquiped;

		public Button buttonEdit;
		public Button buttonRemove;
		public Button buttonPlay;

		private int _panelId;
		private int _charId;

		internal Signal<int> editCharDataSignal = new Signal<int>();
		internal Signal<int> removeCharPanelSignal = new Signal<int>();
		internal Signal<int> startGameSceneSignal = new Signal<int>();

		internal void init(strangeetnix.game.IUserCharInfoVO userCharInfoVO, int panelId1)
		{
			_panelId = panelId1;
			_charId = userCharInfoVO.id;

			textName.text = (userCharInfoVO != null) ? TITLE_NAME + userCharInfoVO.name + ": " + userCharInfoVO.level + " level": TITLE_NAME;
			textHp.text = (userCharInfoVO != null) ? TITLE_HP + userCharInfoVO.hp : TITLE_HP;
			textExp.text = (userCharInfoVO != null) ? TITLE_EXP + userCharInfoVO.exp : TITLE_EXP;
			textWeapon.text = (userCharInfoVO != null) ? TITLE_WEAPON + userCharInfoVO.weapon : TITLE_WEAPON;
			textItem1.text = (userCharInfoVO != null) ? TITLE_ITEM1 + userCharInfoVO.item2 : TITLE_ITEM1;
			textItem2.text = (userCharInfoVO != null) ? TITLE_ITEM2 + userCharInfoVO.item3 : TITLE_ITEM2;
			textEquiped.text = (userCharInfoVO != null) ? TITLE_EQUIPED + userCharInfoVO.equiped : TITLE_EQUIPED;

			buttonEdit.onClick.AddListener (editItem);
			buttonRemove.onClick.AddListener (removeItem);
			buttonPlay.onClick.AddListener (play);
		}

		private void editItem()
		{
			editCharDataSignal.Dispatch (_charId);
		}

		private void removeItem()
		{
			removeCharPanelSignal.Dispatch (_panelId);
		}

		private void play()
		{
			startGameSceneSignal.Dispatch (_charId);
		}
	}
}

