using System;
using UnityEngine;

namespace strangeetnix.game
{
	public class WeaponModel : UserModel, IWeaponModel
	{
		public IEquipedVO equipedVO { get; set; }
		public IWeaponVO weaponVO { get; set; }
		public IItemVO item2VO { get; set; }
		public IItemVO item3VO { get; set; }

		public int damage { get { return Mathf.Max (0, _damage + char_str);}}
		public int cooldown { get { return Mathf.Max (0, _cooldown - char_dex);}}

		private int _damage = 0;
		private int _cooldown = 0;

		public WeaponModel (int setId, IGameConfig gameConfig1) : base (setId, gameConfig1)
		{
			updateAllVO ();
		}

		private void updateAllVO()
		{
			updateWeaponVO ();
			updateItem2VO ();
			updateItem3VO ();
			updateEquipedVO ();
		}

		public void updateWeaponVO()
		{
			weaponVO = gameConfig.weaponConfig.getWeaponVOById (userCharVO.weaponId);

			if (weaponVO != null) {
				_damage = weaponVO.damage;
				_cooldown = weaponVO.cooldown;
			}
		}

		public void updateEquipedVO()
		{
			equipedVO = gameConfig.equipedConfig.getEquipedVOById (userCharVO.equipedId);
		}

		public void updateItem2VO()
		{
			item2VO = gameConfig.itemConfig.getItemVOById (userCharVO.itemId2);
		}

		public void updateItem3VO()
		{
			item3VO = gameConfig.itemConfig.getItemVOById (userCharVO.itemId3);
		}
	}
}

