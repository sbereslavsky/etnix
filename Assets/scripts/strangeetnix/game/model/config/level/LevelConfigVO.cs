using System;

namespace strangeetnix.game
{
	public class LevelConfigVO : ILevelConfigVO
	{
		public int id { get; private set; }
		public bool hasEnemy { get; private set; }

		public LevelConfigVO (int id1, bool hasEnemy1)
		{
			id = id1;
			hasEnemy = hasEnemy1;
		}
	}
}

