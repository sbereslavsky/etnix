using System;

namespace strangeetnix.game
{
	public interface IUserModel
	{
		IUserCharVO userCharVO { get; }

		int id { get; }

		int hp { get; set; }
		int startHp { get; }
		int exp { get; set; }
		int expStart { get; }
		int expEnd { get; }
		int level { get; set; }

		int char_str { get; }
		int char_dex { get; }
		int char_hp { get; }

		bool levelUp { get; set; }

		void addExp (int value);
		void addHp (int value);
		void decHp (int value);
		void resetHp ();

		void updateHp();

		void setLevelDataVO ();
	}
}

