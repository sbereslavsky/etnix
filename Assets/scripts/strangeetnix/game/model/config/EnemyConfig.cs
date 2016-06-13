using System;
using System.Collections.Generic;

namespace strangeetnix.game
{
	public class EnemyConfig : IEnemyConfig
	{
		public List<IEnemyVO> list { get; private set;}

		public EnemyConfig (JSONObject value)
		{
			list = new List<IEnemyVO> (value.Count);
			for (byte i = 0; i < value.Count; i++) {
				list.Add(new EnemyVO (value [i]));
			}
		}

		public IEnemyVO getEnemyVOById(int value) 
		{
			IEnemyVO result = null;
			for (byte i = 0; i < list.Count; i++) {
				if (list [i].id == value) {
					result = list [i];
				}
			}

			return result;
		}
	}
}