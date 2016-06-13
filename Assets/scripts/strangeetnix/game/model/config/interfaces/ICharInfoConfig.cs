using System;
using System.Collections;
using System.Collections.Generic;

namespace strangeetnix.game
{
	public interface ICharInfoConfig
	{
		ICharInfoVO getCharInfoVOById (int id);
		List<string> names { get; }
		string getNameById (int id);
		float getSpeedById (int id);
	}
}