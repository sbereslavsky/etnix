using System;
using System.Collections.Generic;

namespace strangeetnix.game
{
	public class CharABConfig : ICharABConfig
	{
		private List<ICharABVO> _list;

		public CharABConfig (JSONObject value)
		{
			_list = new List<ICharABVO> (value.Count);
			for (byte i = 0; i < value.Count; i++) {
				_list.Add(new CharABVO (value [i]));
			}
		}
	}
}
