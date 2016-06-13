using System;
using System.Collections;
using System.Collections.Generic;

namespace strangeetnix.game
{
	public interface IEnemyConfig
	{
		List<IEnemyVO> list { get;}
		IEnemyVO getEnemyVOById (int value);
	}
}
