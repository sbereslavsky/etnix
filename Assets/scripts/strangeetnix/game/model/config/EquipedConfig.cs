using System;
using System.Collections.Generic;

namespace strangeetnix.game
{
	public class EquipedConfig : IEquipedConfig
	{
		public List<IEquipedVO> list { get; private set;}

		public List<string> info_names { get; private set;}

		public EquipedConfig (JSONObject value)
		{
			list = new List<IEquipedVO> (value.Count);
			info_names = new List<string> (value.Count);
			for (byte i = 0; i < value.Count; i++) {
				list.Add(new EquipedVO (value [i]));
				info_names.Add (list [i].info);
			}
		}

		public string getInfoById(int id)
		{
			string result = "";
			for (byte i = 0; i < list.Count; i++) {
				if (list [i].id == id) {
					result = list [i].info;
					break;
				}
			}

			return result;
		}

		public int getIdByInfo(string value)
		{
			int result = -1;
			for (byte i = 0; i < list.Count; i++) {
				if (list [i].info == value) {
					result = list [i].id;
					break;
				}
			}

			return result;
		}

		public IEquipedVO getEquipedVOById(int id)
		{
			for (byte i = 0; i < list.Count; i++) {
				if (list [i].id == id) {
					return list [i];
				}
			}

			return null;
		}
	}
}