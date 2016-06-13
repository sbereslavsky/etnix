using System;
using System.Collections;
using System.Collections.Generic;

namespace strangeetnix.game
{
	public interface IItemConfig
	{
		List<string> info_names { get; }

		string getInfoById (int id);
		int getIdByInfo (string value);
		IItemVO getItemVOById (int id);
	}
}
