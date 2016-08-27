using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;
using strangeetnix.game;

namespace strangeetnix.ui
{
	public class DialogCharInfoView : View
	{
		private string TITLE_NAME = "Name: ";
		private string TITLE_HP = "Hp: ";
		private string TITLE_EXP = "Exp: ";
		private string TITLE_DAMAGE = "Damage: ";
		private string TITLE_COOLDOWN = "Cooldown: ";
		private string TITLE_WEAPON = "Weapon: ";
		private string TITLE_ITEM1 = "Item1: ";
		private string TITLE_ITEM2 = "Item2: ";
		private string TITLE_EQUIPED = "Equped: ";

		public Text textName;
		public Text textHp;
		public Text textExp;
		public Text textDamage;
		public Text textCooldown;
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

		internal void init(strangeetnix.game.IUserCharInfoVO userCharInfoVO, IWeaponModel weaponModel, int panelId1)
		{
			_panelId = panelId1;
			_charId = userCharInfoVO.id;

			textName.text = (userCharInfoVO != null) ? TITLE_NAME + userCharInfoVO.name + ": " + userCharInfoVO.level + " level": TITLE_NAME;
			textHp.text = (weaponModel != null) ? TITLE_HP + weaponModel.hp : TITLE_HP;
			textExp.text = (weaponModel != null) ? TITLE_EXP + weaponModel.exp : TITLE_EXP;
			textDamage.text = (weaponModel != null) ? TITLE_DAMAGE + weaponModel.damage : TITLE_DAMAGE;
			textCooldown.text = (weaponModel != null) ? TITLE_COOLDOWN + weaponModel.cooldown : TITLE_COOLDOWN;
			textWeapon.text = (userCharInfoVO != null) ? TITLE_WEAPON + userCharInfoVO.weapon : TITLE_WEAPON;
			textItem1.text = (userCharInfoVO != null) ? TITLE_ITEM1 + userCharInfoVO.item2 : TITLE_ITEM1;
			textItem2.text = (userCharInfoVO != null) ? TITLE_ITEM2 + userCharInfoVO.item3 : TITLE_ITEM2;
			textEquiped.text = (userCharInfoVO != null) ? TITLE_EQUIPED + userCharInfoVO.equiped : TITLE_EQUIPED;

			buttonEdit.onClick.AddListener (editItem);
			buttonRemove.onClick.AddListener (removeItem);
			buttonPlay.onClick.AddListener (play);
		}

		internal void initTooltipText(string text)
		{
			TooltipTrigger trigger = this.gameObject.AddComponent<TooltipTrigger> ();
			trigger.text = text;
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

