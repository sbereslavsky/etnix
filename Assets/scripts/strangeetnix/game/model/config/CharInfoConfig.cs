using System;
using System.Collections.Generic;

namespace strangeetnix.game
{
	public class CharInfoConfig : ICharInfoConfig
	{
		private List<ICharInfoVO> _list;

		public List<string> names { get; private set;}

		public CharInfoConfig (JSONObject value)
		{
			_list = new List<ICharInfoVO> (value.Count);
			names = new List<string> (value.Count);
			for (byte i = 0; i < value.Count; i++) {
				_list.Add(new CharInfoVO (value [i]));
				names.Add (_list[i].name);
			}
		}

		public ICharInfoVO getCharInfoVOById (int id)
		{
			ICharInfoVO result = null;
			for (byte i = 0; i < _list.Count; i++) {
				if (_list [i].id == id) {
					result = _list [i];
					break;
				}
			}

			return result;
		}

		public string getNameById (int id)
		{
			ICharInfoVO charInfoVO = getCharInfoVOById (id);
			string result = (charInfoVO != null) ? charInfoVO.name : "";
			return result;
		}

		public float getSpeedById (int id)
		{
			ICharInfoVO charInfoVO = getCharInfoVOById (id);
			float result = (charInfoVO != null) ? charInfoVO.speed : 1f;
			return result;
		}
	}
}