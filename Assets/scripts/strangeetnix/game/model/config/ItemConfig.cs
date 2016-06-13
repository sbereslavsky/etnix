using System;
using System.Collections.Generic;

namespace strangeetnix.game
{
	public class ItemConfig : IItemConfig
	{
		private List<IItemVO> _list;

		public List<string> info_names { get; private set;}

		public ItemConfig (JSONObject value)
		{
			_list = new List<IItemVO> (value.Count);
			info_names = new List<string> (value.Count);
			for (byte i = 0; i < value.Count; i++) {
				_list.Add(new ItemVO (value [i]));
				info_names.Add (_list[i].info);
			}
		}

		public int getIdByInfo(string value)
		{
			int result = 0;
			for (byte i = 0; i < _list.Count; i++) {
				if (_list [i].info == value) {
					result = _list [i].id;
					break;
				}
			}

			return result;
		}

		public IItemVO getItemVOById (int id)
		{
			for (byte i = 0; i < _list.Count; i++) {
				if (_list [i].id == id) {
					return _list [i];
				}
			}

			return null;
		}

		public string getInfoById(int id)
		{
			IItemVO itemVO = getItemVOById (id);
			string result = (itemVO != null) ? itemVO.info : "";
			return result;
		}
	}
}