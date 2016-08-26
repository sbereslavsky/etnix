using System;
using System.Collections.Generic;

namespace strangeetnix.game
{
	public interface IPlayerModel
	{
		int id { get; }
		string name { get; }

		int hp { get; set; }
		int startHp { get; }
		int exp { get; set; }
		int expStart { get; }
		int expEnd { get; }
		int level { get; set; }
		int coins { get; }
		float moveSpeed { get; }
		float moveForce { get; }

		int damage { get; }
		int cooldown { get; }

		bool levelUp { get; }

		ICharAssetVO assetVO { get; }
		IEquipedVO equipedVO { get; set; }
		IWeaponVO weaponVO { get; set; }
		IItemVO item2VO { get; set; }
		IItemVO item3VO { get; set; }

		void addExp (int value);
		void addHp (int value);
		void addCoins (int value);
		void decHp (int value);

		void updateNextExp(IGameConfig gameConfig);
		void resetHp ();
		void updateConfig ();
		void updateCharInfo(IGameConfig gameConfig);
	}
}