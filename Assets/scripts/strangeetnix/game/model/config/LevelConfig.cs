using System;
using UnityEngine;
using System.Collections.Generic;

namespace strangeetnix.game
{
	public class LevelConfig : ILevelConfig
	{
		private List<ILevelConfigVO> _list;

		public LevelConfig ()
		{
			_list = new List<ILevelConfigVO> ();
			_list.Add (new LevelConfigVO (1, false));
			_list.Add (new LevelConfigVO (1001, true));
		}

		public ILevelConfigVO getConfigById(int value)
		{
			for (byte i = 0; i < _list.Count; i++) {
				if (_list [i].id == value) {
					return _list [i];
				}
			}

			return null;
		}
	}
}

