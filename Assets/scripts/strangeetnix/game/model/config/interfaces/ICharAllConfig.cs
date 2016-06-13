using System;
using System.Collections;
using System.Collections.Generic;

namespace strangeetnix.game
{
	public interface ICharAllConfig
	{
		int getLevelChar (int exp);
		int getHpByLevel (int level);

		ICharAllVO getCharAllVOByExp(int exp);
		ICharAllVO getCharAllVOByLevel(int level);
	}
}