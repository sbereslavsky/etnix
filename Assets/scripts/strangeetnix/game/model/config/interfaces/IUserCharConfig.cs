using System;
using System.Collections.Generic;

namespace strangeetnix.game
{
	public interface IUserCharConfig
	{
		List<IUserCharVO> list { get; }
		IUserCharVO getUserCharVOById (int classId);
		JSONObject getJSONObject ();
	}
}