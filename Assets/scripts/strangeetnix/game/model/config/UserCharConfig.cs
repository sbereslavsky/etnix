using System;
using System.Collections.Generic;

namespace strangeetnix.game
{
	public class UserCharConfig : IUserCharConfig
	{
		private string USER_CHAR = "user_char";

		public List<IUserCharVO> list { get; private set;}

		private JSONObject _value;

		public UserCharConfig (JSONObject value)
		{
			_value = value;
			//parse UserData to UserCharVO list
			JSONObject userChars = _value.GetField (USER_CHAR);

			list = new List<IUserCharVO> (userChars.Count);
			for (byte i = 0; i < userChars.Count; i++) {
				list.Add(new UserCharVO (userChars [i]));
			}
		}

		public IUserCharVO getUserCharVOById(int classId)
		{
			for (byte i = 0; i < list.Count; i++) {
				if (list [i].classId == classId) {
					return list [i];
				}
			}

			return null;
		}

		public JSONObject getJSONObject()
		{
			for (byte i = 0; i < list.Count; i++) {
				list [i].updateData ();
			}

			return _value;
		}
	}
}