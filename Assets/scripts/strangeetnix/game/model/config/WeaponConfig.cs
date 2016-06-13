using System;
using System.Collections.Generic;

namespace strangeetnix.game
{
	public class WeaponConfig : IWeaponConfig
	{
		private List<IWeaponVO> _list;

		public WeaponConfig (JSONObject value)
		{
			_list = new List<IWeaponVO> (value.Count);
			for (byte i = 0; i < value.Count; i++) {
				_list.Add(new WeaponVO (value [i]));
			}
		}

		public List<string> getInfoListByOwnerId(int owner_id)
		{
			List<string> result = new List<string> ();
			for (byte i = 0; i < _list.Count; i++) {
				if (_list [i].owner_id == owner_id) {
					result.Add(_list [i].info);
				}
			}

			return result;
		}

		public int getIdByInfo(string value)
		{
			for (byte i = 0; i < _list.Count; i++) {
				if (_list [i].info == value) {
					return _list [i].id;
				}
			}

			return 0;
		}

		public string getInfoById(int id)
		{
			IWeaponVO weaponVO = getWeaponVOById (id);
			string result = (weaponVO != null) ?  weaponVO.info : "";
			return result;
		}

		public IWeaponVO getWeaponVOById(int id) 
		{
			for (byte i = 0; i < _list.Count; i++) {
				if (_list [i].id == id) {
					return _list [i];
				}
			}

			return null;
		}
	}
}