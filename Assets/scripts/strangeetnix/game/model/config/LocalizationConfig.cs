using System;
using System.Collections.Generic;

namespace strangeetnix.game
{
	public class LocalizationConfig : ILocalizationConfig
	{
		private List<ILocalizationVO> _list;
		//private string LANG = "EN";

		public LocalizationConfig (JSONObject value)
		{
			_list = new List<ILocalizationVO> (value.Count);
			for (byte i = 0; i < value.Count; i++) {
				_list.Add(new LocalizationVO (value [i]));
			}
		}

		public string getTextByKey(string key)
		{
			string result = "";
			for (byte i = 0; i < _list.Count; i++) {          
				if ( _list [i].key == key) {
					result = _list[i].en;
					break;
				}
			}
			return result;
		}
	}
}
