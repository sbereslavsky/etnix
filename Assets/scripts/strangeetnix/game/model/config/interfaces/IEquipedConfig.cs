using System;
using System.Collections.Generic;

namespace strangeetnix.game
{
	public interface IEquipedConfig
	{
		List<IEquipedVO> list { get;}
		List<string> info_names { get; }
		string getInfoById (int id);
		int getIdByInfo (string value);

		IEquipedVO getEquipedVOById (int id);
	}
}