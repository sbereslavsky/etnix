using System;

namespace strangeetnix.game
{
	public interface IWeaponModel : IUserModel
	{
		IEquipedVO equipedVO { get; set; }
		IWeaponVO weaponVO { get; set; }
		IItemVO item2VO { get; set; }
		IItemVO item3VO { get; set; }

		int damage { get; }
		int cooldown { get; }

		void updateWeaponVO();
		void updateEquipedVO();
		void updateItem2VO();
		void updateItem3VO();
	}
}

