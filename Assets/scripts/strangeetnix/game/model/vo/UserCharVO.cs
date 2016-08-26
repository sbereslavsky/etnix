using System;

namespace strangeetnix.game
{
	public class UserCharVO : ParseJSONObject, IUserCharVO
	{
		private string CHAR_ID 			= "charid";
		private string CHAR_CLASS 		= "charclass";
		private string CHAR_EQUIPED 	= "charequeped";
		private string CHAR_IS_ACTIVE	= "isactiv";
		private string CHAR_EXP 		= "charexp";
		private string CHAR_WEAPON_ID 	= "charweapon";
		private string CHAR_ITEM_ID2 	= "charitem1";
		private string CHAR_ITEM_ID3 	= "charitem2";
		private string CHAR_COINS 		= "charcoins";
		//private string CHAR_WAVE_ID 	= "charwave";

		public int id { get; private set; }
		public int classId { get; private set; }
		public int equipedId { get; set; }
		public int isActive { get; set; }
		public int exp { get; set; }
		public int weaponId { get; set; }
		public int itemId2 { get; set; }
		public int itemId3 { get; set; }
		public int coins { get; set; }
		//public int waveId { get; set; }

		private JSONObject _value;

		public UserCharVO (JSONObject value)
		{
			_value = value;

			id = getInt32(_value, CHAR_ID);
			classId = getInt32(_value, CHAR_CLASS);
			equipedId = getInt32(_value, CHAR_EQUIPED);
			isActive = getInt32(_value, CHAR_IS_ACTIVE);
			exp = getInt32(_value, CHAR_EXP);
			weaponId = getInt32(_value, CHAR_WEAPON_ID);
			itemId2 = getInt32(_value, CHAR_ITEM_ID2);
			itemId3 = getInt32(_value, CHAR_ITEM_ID3);
			coins = getInt32(_value, CHAR_COINS);
			//waveId = getInt32(_value, CHAR_WAVE_ID);
		}

		public IUserCharInfoVO getUserCharInfoVO(IGameConfig gameConfig)
		{
			UserCharInfoVO userCharInfoVO = new UserCharInfoVO ();
			userCharInfoVO.id = id;
			userCharInfoVO.name = gameConfig.charInfoConfig.getNameById (classId);
			userCharInfoVO.exp = exp;
			userCharInfoVO.level = gameConfig.charAllConfig.getLevelChar (exp);
			userCharInfoVO.hp = gameConfig.charAllConfig.getHpByLevel (userCharInfoVO.level);
			userCharInfoVO.equiped = gameConfig.equipedConfig.getInfoById(equipedId);
			userCharInfoVO.weapon = gameConfig.weaponConfig.getInfoById(weaponId);
			userCharInfoVO.item2 = gameConfig.itemConfig.getInfoById(itemId2);
			userCharInfoVO.item3 = gameConfig.itemConfig.getInfoById(itemId3);
			//userCharInfoVO.wave = gameConfig.waveConfig.getEncounterListById (waveId);
			return userCharInfoVO;
		}

		public void updateData()
		{
			setIntToField (_value, CHAR_EQUIPED, equipedId);
			setIntToField (_value, CHAR_EXP, exp);
			setIntToField (_value, CHAR_WEAPON_ID, weaponId);
			setIntToField (_value, CHAR_ITEM_ID2, itemId2);
			setIntToField (_value, CHAR_ITEM_ID3, itemId3);
			setIntToField (_value, CHAR_COINS, coins);
			//setIntToField (_value, CHAR_WAVE_ID, waveId);

			int active = (classId > 0) ? 1 : 0;
			setIntToField (_value, CHAR_IS_ACTIVE, active);
		}
	}
}