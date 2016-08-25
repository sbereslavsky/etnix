using System;

namespace strangeetnix.game
{
	public interface IUserCharVO
	{
		int id { get;}
		int classId { get;}
		int equipedId { get; set; }
		int isActive { get; set; }
		int exp { get; set; }
		int weaponId { get; set; }
		int itemId2 { get; set; }
		int itemId3 { get; set; }
		int coins { get; set; }
		//int waveId { get; set; }

		IUserCharInfoVO getUserCharInfoVO(IGameConfig gameConfig);
		void updateData();
	}
}