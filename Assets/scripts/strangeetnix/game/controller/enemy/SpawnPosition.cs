using System;

namespace strangeetnix.game
{
	public class SpawnPosition
	{
		public float position { get; private set; }
		public bool bothSides { get; private set; }

		public SpawnPosition (float position1, bool bothSides1)
		{
			position = position1;
			bothSides = bothSides1;
		}
	}
}

