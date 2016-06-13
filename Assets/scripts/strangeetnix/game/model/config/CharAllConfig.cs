using System;
using System.Collections.Generic;

namespace strangeetnix.game
{
	public class CharAllConfig : ICharAllConfig
	{
		private List<ICharAllVO> _list;

		public CharAllConfig (JSONObject value)
		{
			_list = new List<ICharAllVO> (value.Count);
			for (byte i = 0; i < value.Count; i++) {
				_list.Add(new CharAllVO (value [i]));
			}
		}

		public ICharAllVO getCharAllVOByExp(int exp)
		{
			ICharAllVO charAllVO = null;
			for (byte i = 0; i < _list.Count; i++) {          
				if ( _list [i].exp_next <= exp) {
					charAllVO = _list[i];
				}
			}
			return charAllVO;
		}

		public ICharAllVO getCharAllVOByLevel(int level)
		{
			for (byte i = 0; i < _list.Count; i++) {          
				if ( _list [i].level_id == level) {
					return _list[i];
				}
			}
			return null;
		}

		public int getLevelChar(int exp)
		{
			//получение уровня персонажа в зависимости от количества опыта
			List<int> levels = new List<int>();
			int level_id;
			ICharAllVO charAllVO;
			for (byte i = 0; i < _list.Count; i++) {          
				charAllVO = _list [i];
				if ( charAllVO.exp_next <= exp) {
					level_id = charAllVO.level_id;
					levels.Add(level_id);
				}
			}
			levels.Sort();
			return levels[levels.Count - 1];
		}

		public int getHpByLevel(int level)
		{
			for (byte i = 0; i < _list.Count; i++) {          
				if ( _list [i].level_id == level) {
					return _list [i].ch1_hp;
				}
			}

			return 0;
		}
	}
}