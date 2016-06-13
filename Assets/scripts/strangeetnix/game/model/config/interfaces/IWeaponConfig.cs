using System;
using System.Collections.Generic;

namespace strangeetnix.game
{
	public interface IWeaponConfig
	{
		List<string> getInfoListByOwnerId (int owner_id);
		string getInfoById (int id);
		int getIdByInfo (string value);
		IWeaponVO getWeaponVOById (int id);
	}
}